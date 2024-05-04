using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    public static event Action<SwipeDirection> OnSwipe;
    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public SwipeDirection swipeDirection;
    [SerializeField] private GameManager gameManager;

    [SerializeField] private bool detectSwipeOnlyAfterRelease = false;
    [SerializeField] private float minDistanceForSwipe = 20f;
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private bool waiting;

    private void Update()
    {
        if(Input.touchCount == 1 && !gameManager.canMove)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                waiting = true;
                fingerDownPosition = touch.position;
                fingerUpPosition = touch.position;
            }
            else if(!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved && waiting)
            {
                fingerUpPosition = touch.position;
                DetectSwipe();
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                waiting = true;
            }
        }
    }

    void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            waiting = false;
            Vector2 swipeDirection = fingerUpPosition - fingerDownPosition;

            float angle = Mathf.Atan2(swipeDirection.y, swipeDirection.x) * Mathf.Rad2Deg;

            if (Mathf.Abs(angle) < 15f || Mathf.Abs(angle - 180) < 15f || Mathf.Abs(angle + 180) < 15f)
            {
                var direction = swipeDirection.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
            else if (Mathf.Abs(angle - 90) < 15f || Mathf.Abs(angle + 90) < 15f)
            {
                var direction = swipeDirection.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            
            fingerDownPosition = fingerUpPosition;
        }
    }

    private bool SwipeDistanceCheckMet()
    {
        return Vector3.Distance(fingerDownPosition, fingerUpPosition) > minDistanceForSwipe;
    }

    private void SendSwipe(SwipeDirection direction)
    {
        OnSwipe?.Invoke(direction);
        swipeDirection = direction;
    }
}
