using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMachine : MonoBehaviour
{
    public static event Action OnSetCameraFollowByDoor;
    public GameObject power;
    public float timeOpen;
    [SerializeField] private float timeElapsed;
    [SerializeField] private Transform door;
    [SerializeField] private Transform showPoint;
    [SerializeField] private Transform hidePoint;
    [SerializeField] private Transform[] listInputWaypoints;
    private float moveSpeed = 20;

    private void Update()
    {
        if(timeOpen > 0)
        {
            timeElapsed += Time.deltaTime;
            if(door.position != hidePoint.position)
            {
                door.position = Vector2.MoveTowards(door.position, hidePoint.position, moveSpeed * Time.deltaTime);
            }
            
            if (timeElapsed >= 1f)
            {
                timeOpen --;
                timeElapsed = 0;
            }

            if(timeOpen <= 0) 
            {
                timeOpen = 0;
                StartCoroutine(LockDoor());
            }
        }
    }

    public void ReversePowerToSource(GameObject currentPower, GameObject source)
    {
        power = currentPower;
        PowerToDoor();
        StartCoroutine(PowerReverseMoving(source));
    }

    public void PowerToDoor()
    {
        timeOpen = 5;
    }

    private IEnumerator PowerReverseMoving(GameObject source)
    {
        Power.waiting = true;
        int indexReverse = listInputWaypoints.Length - 1;
        while(power.transform.position != listInputWaypoints[0].position)
        {
            power.transform.position = Vector2.MoveTowards(power.transform.position, listInputWaypoints[indexReverse].position, moveSpeed * Time.deltaTime);
            float distance = Vector2.Distance(power.transform.position, listInputWaypoints[indexReverse].position);
            if(distance < 0.1f && indexReverse > 0)
            {
                indexReverse--;
            }
            yield return null;
        }

        Bulb bulb = source.GetComponent<Bulb>();
        if(bulb)
        {
            bulb.isOn = true;
            bulb.SetPower(true);
        }

        OnSetCameraFollowByDoor?.Invoke();
        power = null;
        Power.waiting = false;
    }

    private IEnumerator LockDoor()
    {
        while(door.position != showPoint.position)
        {
            door.position = Vector2.MoveTowards(door.position, showPoint.position, moveSpeed * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
