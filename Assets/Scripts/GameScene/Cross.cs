using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    public static event Action<Bulb> OnSetPowerBulbByCross;
    public static event Action OnSetCameraFollowByCross;
    public GameObject power;
    [SerializeField] private CrossDirection[] listCrossDirection;
    private float moveSpeed = 20;

    private void OnEnable()
    {
        SwipeDetector.OnSwipe += MovePower;
    }

    private void MovePower(SwipeDetector.SwipeDirection swipeDirection)
    {
        if(!power || Power.waiting || GameManager.pause) return;
        foreach(CrossDirection crossDirection in listCrossDirection)
        {
            if(crossDirection.swipeDirection == swipeDirection)
            {
                StartCoroutine(PowerMoving(crossDirection.listWaypoints));
            }
        }
    }

    private IEnumerator PowerMoving(Transform[] listWaypoints)
    {
        Power.waiting = true;
        OnSetCameraFollowByCross?.Invoke();
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
        InputMachine inputMachine = listWaypoints[listWaypoints.Length - 1].GetComponent<InputMachine>();
        if(bulbTarget)
        {
            bulbTarget.isOn = true;
            bulbTarget.SetPower(true);
            OnSetPowerBulbByCross?.Invoke(bulbTarget);
            power = null;
            Power.waiting = false;
        }
        else if(cross)
        {
            cross.power = power;
            power = null;
            Power.waiting = false;
        }
        else if(inputMachine)
        {
            if(inputMachine.inputType == InputMachine.InputType.Charged)
            {
                inputMachine.chargedMachine.ReversePowerToSource(power, gameObject);
            }
            else if(inputMachine.inputType == InputMachine.InputType.AndGate1)
            {
                inputMachine.andGate.ReversePowerToSource(power, gameObject, 1);
            }
            else if(inputMachine.inputType == InputMachine.InputType.AndGate2)
            {
                inputMachine.andGate.ReversePowerToSource(power, gameObject, 2);
            }
            else if(inputMachine.inputType == InputMachine.InputType.Door)
            {
                inputMachine.doorMachine.ReversePowerToSource(power, gameObject);
            }
            else if(inputMachine.inputType == InputMachine.InputType.Wall)
            {
                inputMachine.wallMachine.ReversePowerToSource(power, gameObject);
            }
        }
    }

    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= MovePower;
    }
}

[Serializable]
public class CrossDirection
{
    public SwipeDetector.SwipeDirection swipeDirection;
    public Transform[] listWaypoints;
}
