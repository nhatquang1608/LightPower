using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulb : MonoBehaviour
{
    public static event Action<Bulb> OnSetPowerBulb;
    public bool isOn;
    [SerializeField] private SwipeDetector.SwipeDirection direction;
    [SerializeField] private GameObject onBulb;
    [SerializeField] private GameObject offBulb;
    [SerializeField] private GameObject power;
    [SerializeField] private Transform[] listWaypoints;
    private float moveSpeed = 20;

    private void Awake()
    {
        SetPower(isOn);
    }

    private void OnEnable()
    {
        SwipeDetector.OnSwipe += MovePower;
    }

    private void MovePower(SwipeDetector.SwipeDirection swipeDirection)
    {
        if(!isOn || listWaypoints.Length == 0 || swipeDirection != direction) return;

        isOn = false;
        SetPower(false);
        power = GameObject.FindGameObjectWithTag("Power");
        if(power)
        {
            StartCoroutine(PowerMoving());
        }
    }

    private IEnumerator PowerMoving()
    {
        int index = 0;
        while(power.transform.position != listWaypoints[listWaypoints.Length - 1].position)
        {
            power.transform.position = Vector2.MoveTowards(power.transform.position, listWaypoints[index].position, moveSpeed * Time.deltaTime);
            float distance = Vector2.Distance(power.transform.position, listWaypoints[index].position);
            if(distance < 0.1f && index < listWaypoints.Length - 1)
            {
                index++;
            }
            yield return null;
        }

        Bulb bulbTarget = listWaypoints[listWaypoints.Length - 1].GetComponent<Bulb>();
        Cross cross = listWaypoints[listWaypoints.Length - 1].GetComponent<Cross>();
        if(bulbTarget)
        {
            bulbTarget.isOn = true;
            bulbTarget.SetPower(true);
            OnSetPowerBulb?.Invoke(bulbTarget);
        }
        else if(cross)
        {
            cross.power = power;
        }

        power = null;
    }

    public void SetPower(bool on)
    {
        onBulb.SetActive(on);
        isOn = on;
    }

    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= MovePower;
    }
}
