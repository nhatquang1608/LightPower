using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMachine : MonoBehaviour
{
    public enum InputType
    {
        Charged,
        AndGate1,
        AndGate2,
        Door,
        Wall
    }

    public InputType inputType;
    public ChargedMachine chargedMachine;
    public AndGate andGate;
    public DoorMachine doorMachine;
    public WallMachine wallMachine;
}
