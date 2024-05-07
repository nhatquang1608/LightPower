using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private enum ControlType
    {
        Left,
        Right,
        Up
    }

    [SerializeField] private ControlType controlType;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch(controlType)
        {
            case ControlType.Left:
                CharacterManager.moveInput = -1;
                break;
            case ControlType.Right:
                CharacterManager.moveInput = 1;
                break;
            case ControlType.Up:
                CharacterManager.jumpInput = true;
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch(controlType)
        {
            case ControlType.Left:
                CharacterManager.moveInput = 0;
                break;
            case ControlType.Right:
                CharacterManager.moveInput = 0;
                break;
        }
    }
}
