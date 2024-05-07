using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMachine : MonoBehaviour
{
    public bool pressed;
    [SerializeField] private float timeElapsed;
    [SerializeField] private int objectsPressed;
    [SerializeField] private GameObject smallPowerPrefab;
    [SerializeField] private Transform output;
    [SerializeField] private Transform warningButton;
    [SerializeField] private Transform showPoint;
    [SerializeField] private Transform hidePoint;
    [SerializeField] private Transform[] listOutputWaypoints;
    private float moveSpeed = 20;

    private void Update()
    {
        if(pressed)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= 1f)
            {
                timeElapsed = 0;
                StartCoroutine(SmallEnergyTransmission());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.transform.tag == "Bulb" || collider.transform.tag == "Box")
        {
            StartCoroutine(ShowHideButton(hidePoint));
            pressed = true;
            objectsPressed++;
            StartCoroutine(SmallEnergyTransmission());
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.transform.tag == "Bulb" || collider.transform.tag == "Box")
        {
            pressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.transform.tag == "Bulb" || collider.transform.tag == "Box")
        {
            objectsPressed--;
            if(objectsPressed <= 0)
            {
                StartCoroutine(ShowHideButton(showPoint));
                pressed = false;
            }
        }
    }

    private IEnumerator ShowHideButton(Transform point)
    {
        while(warningButton.position != point.position)
        {
            warningButton.position = Vector2.MoveTowards(warningButton.position, point.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
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
