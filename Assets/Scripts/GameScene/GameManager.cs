using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject powerPrefabs;
    [SerializeField] private GameObject bulb;
    [SerializeField] private GameObject power;
    [SerializeField] private GameObject[] listBackgrounds;
    [SerializeField] private GameObject[] listLevels;
    private float radius = 5f;
    public bool canMove;

    private void Start()
    {
        bulb = GameObject.FindGameObjectWithTag("Input");
        power = Instantiate(powerPrefabs, bulb.transform.position, Quaternion.identity);
    }

    private void OnEnable()
    {
        Bulb.OnSetPowerBulb += SetPowerBulb;
        Cross.OnSetPowerBulbByCross += SetPowerBulb;
    }
    
    private void Update()
    {
        if (Input.touchCount == 1)
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
                        bulb.GetComponent<Bulb>().SetPower(true);
                    }

                    power.transform.position = bulb.transform.position;
                    Power powerManager = power.GetComponent<Power>();
                    powerManager.SetLineRenderer(bulb, power, false);
                    canMove = false;

                    if(hit.collider != null && hit.collider.CompareTag("Output"))
                    {
                        powerManager.PowerOff();
                    }
                }
            }
        }
    }

    private void SetPowerBulb(Bulb bulbTarget)
    {
        bulb = bulbTarget.gameObject;
        if(bulb.transform.CompareTag("Output"))
        {
            Power powerManager = power.GetComponent<Power>();
            powerManager.PowerOff();
        }
    }

    private void OnDisable()
    {
        Bulb.OnSetPowerBulb -= SetPowerBulb;
        Cross.OnSetPowerBulbByCross -= SetPowerBulb;
    }
}
