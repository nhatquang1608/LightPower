using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool pause;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public GameObject bulb;
    public GameObject power;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private CanvasGroup gameOver;
    [SerializeField] private GameObject powerPrefabs;
    [SerializeField] private GameObject listButtonControl;
    [SerializeField] private GameObject[] listLevels;
    private float radius = 4f;
    public bool canMove;

    private void Awake()
    {
        pause = false;
        listLevels[ListLevels.Instance.levelIndex].SetActive(true);
        CharacterManager.moveInput = 0;
        CharacterManager.jumpInput = false;
    }

    private void Start()
    {
        bulb = GameObject.FindGameObjectWithTag("Input");
        power = Instantiate(powerPrefabs, bulb.transform.position, Quaternion.identity);

        cinemachineVirtualCamera.Follow = bulb.transform;
    }

    private void OnEnable()
    {
        Bulb.OnSetPowerBulb += SetPowerBulb;
        Cross.OnSetPowerBulbByCross += SetPowerBulb;

        Bulb.OnSetCameraFollow += OnSetCameraFollow;
        Cross.OnSetCameraFollowByCross += OnSetCameraFollow;

        ChargedMachine.OnSetCameraFollowByCharged += OnSetCameraFollowBulb;
        AndGate.OnSetCameraFollowByAndGate += OnSetCameraFollowBulb;
        DoorMachine.OnSetCameraFollowByDoor += OnSetCameraFollowBulb;

        CharacterManager.OnSetButtonControl += OnSetButtonControl;
    }
    
    private void Update()
    {
        if (Input.touchCount == 1 && !pause)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if(hit.collider != null && (hit.collider.CompareTag("Bulb") || hit.collider.CompareTag("Input")))
                {
                    Bulb bulbTouch = hit.collider.GetComponent<Bulb>();
                    if(bulbTouch.isOn) canMove = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if(canMove)
                {
                    Vector3 offset = touchPosition - bulb.transform.position;
                    if (offset.magnitude > radius)
                    {
                        offset = offset.normalized * radius;
                    }
                    power.transform.position = bulb.transform.position + offset;
                    Power powerManager = power.GetComponent<Power>();
                    powerManager.SetLineRenderer(bulb, power, true);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if(canMove)
                {
                    RaycastHit2D hit = Physics2D.Raycast(power.transform.position, Vector2.zero);

                    if(hit.collider != null && (hit.collider.CompareTag("Bulb") || hit.collider.CompareTag("Input") || hit.collider.CompareTag("Output")))
                    {
                        bulb.GetComponent<Bulb>().SetPower(false);
                        bulb = hit.collider.gameObject;
                        OnSetCameraFollowBulb();
                        bulb.GetComponent<Bulb>().SetPower(true);
                    }

                    power.transform.position = bulb.transform.position;
                    Power powerManager = power.GetComponent<Power>();
                    powerManager.SetLineRenderer(bulb, power, false);
                    canMove = false;

                    if(hit.collider != null && hit.collider.CompareTag("Output"))
                    {
                        powerManager.PowerOff();
                        StartCoroutine(Fade(gameOver, 1f, 1f));
                    }
                }
            }
        }
    }

    private void OnSetButtonControl(bool show)
    {
        listButtonControl.SetActive(show);
    }

    private void OnSetCameraFollow()
    {
        cinemachineVirtualCamera.Follow = power.transform;
    }

    private void OnSetCameraFollowBulb()
    {
        cinemachineVirtualCamera.Follow = bulb.transform;
    }

    private void SetPowerBulb(Bulb bulbTarget)
    {
        bulb = bulbTarget.gameObject;
        OnSetCameraFollowBulb();
        if(bulb.transform.CompareTag("Output"))
        {
            Power powerManager = power.GetComponent<Power>();
            powerManager.PowerOff();
            StartCoroutine(Fade(gameOver, 1f, 1f));
        }
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;

        if(ListLevels.Instance.levelIndex < listLevels.Length-1)
        {
            ListLevels.Instance.levelIndex ++;
        }
        else
        {
            ListLevels.Instance.levelIndex = 0;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToHome()
    {
        ListLevels.Instance.levelIndex = 0;
        SceneManager.LoadScene("TopScene");
    }

    public void Pause()
    {
        pause = true;
        pausePanel.SetActive(true);
    }

    private void OnDisable()
    {
        Bulb.OnSetPowerBulb -= SetPowerBulb;
        Cross.OnSetPowerBulbByCross -= SetPowerBulb;

        Bulb.OnSetCameraFollow -= OnSetCameraFollow;
        Cross.OnSetCameraFollowByCross -= OnSetCameraFollow;

        ChargedMachine.OnSetCameraFollowByCharged -= OnSetCameraFollowBulb;
        AndGate.OnSetCameraFollowByAndGate -= OnSetCameraFollowBulb;
        DoorMachine.OnSetCameraFollowByDoor -= OnSetCameraFollowBulb;

        CharacterManager.OnSetButtonControl -= OnSetButtonControl;
    }
}
