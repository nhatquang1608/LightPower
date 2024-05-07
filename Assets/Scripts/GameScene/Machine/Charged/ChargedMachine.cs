using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedMachine : MonoBehaviour
{
    public static event Action OnSetCameraFollowByCharged;
    public float time;
    [SerializeField] private float timeElapsed;
    public GameObject power;
    public Transform input;
    public Transform output;

    [SerializeField] private GameObject smallPowerPrefab;
    [SerializeField] private Transform[] listInputWaypoints;
    [SerializeField] private Transform[] listOutputWaypoints;
    private float moveSpeed = 20;

    private void Update()
    {
        if(time > 0)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= 1f)
            {
                time --;
                timeElapsed = 0;
                StartCoroutine(SmallEnergyTransmission());
            }

            if(time <= 0) time = 0;
        }
    }

    public void ReversePowerToSource(GameObject currentPower, GameObject source)
    {
        power = currentPower;
        PowerToCharged();
        StartCoroutine(PowerReverseMoving(source));
    }

    public void PowerToCharged()
    {
        time = 7;
        StartCoroutine(SmallEnergyTransmission());
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

        OnSetCameraFollowByCharged?.Invoke();
        power = null;
        Power.waiting = false;
    }

    private IEnumerator SmallEnergyTransmission()
    {
        int index = 0;
        GameObject smallPower = Instantiate(smallPowerPrefab, output.position, Quaternion.identity);
        while(smallPower.transform.position != listOutputWaypoints[listOutputWaypoints.Length - 1].position)
        {
            smallPower.transform.position = Vector2.MoveTowards(smallPower.transform.position, listOutputWaypoints[index].position, moveSpeed * Time.deltaTime);
            float distance = Vector2.Distance(smallPower.transform.position, listOutputWaypoints[index].position);
            if(distance < 0.1f && index < listOutputWaypoints.Length - 1)
            {
                index++;
            }
            yield return null;
        }

        InputMachine inputMachine = listOutputWaypoints[listOutputWaypoints.Length - 1].GetComponent<InputMachine>();
        if(inputMachine)
        {
            if(inputMachine.inputType == InputMachine.InputType.Charged)
            {
                inputMachine.chargedMachine.PowerToCharged();
            }
            else if(inputMachine.inputType == InputMachine.InputType.AndGate1)
            {
                inputMachine.andGate.PowerToAndGate(1);
            }
            else if(inputMachine.inputType == InputMachine.InputType.AndGate2)
            {
                inputMachine.andGate.PowerToAndGate(2);
            }
            else if(inputMachine.inputType == InputMachine.InputType.Door)
            {
                inputMachine.doorMachine.PowerToDoor();
            }
            else if(inputMachine.inputType == InputMachine.InputType.Wall)
            {
                inputMachine.wallMachine.PowerToWall();
            }
        }
        Destroy(smallPower);
    }
}
