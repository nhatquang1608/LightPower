using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallMachine : MonoBehaviour
{
    public static event Action OnSetCameraFollowByWall;
    public float time;
    public GameObject power;

    [SerializeField] private Transform[] listInputWaypoints;
    [SerializeField] private Wall[] listHiddenWalls;
    [SerializeField] private Wall[] listShowedWalls;
    private float moveSpeed = 20;

    private void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
            if(time <= 0) 
            {
                time = 0;
                ShowHideWalls(false);
            }
        }
    }
    
    public void ReversePowerToSource(GameObject currentPower, GameObject source)
    {
        power = currentPower;
        PowerToWall();
        StartCoroutine(PowerReverseMoving(source));
    }

    private void ShowHideWalls(bool showHiddenWalls)
    {
        if(listHiddenWalls.Length > 0)
        {
            foreach(Wall wall in listHiddenWalls)
            {
                wall.ShowHide(showHiddenWalls);
            }
        }

        if(listShowedWalls.Length > 0)
        {
            foreach(Wall wall in listShowedWalls)
            {
                wall.ShowHide(!showHiddenWalls);
            }
        }
    }

    public void PowerToWall()
    {
        time = 2;
        ShowHideWalls(true);
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

        OnSetCameraFollowByWall?.Invoke();
        power = null;
        Power.waiting = false;
    }
}
