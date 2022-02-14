using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InputManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    public enum Direction { Left, Up, Right, Down, None }

    Direction direction;
    Vector2 startPos, endPos;
    public float swipeThreshold = 100f;
    bool draggingStarted;
    //public Action<Direction> onSwipeDetected;
    public float xManager = 0; // We use this to convert for CharacterController without needing to do it everyframe but just every swipe
    public float yManager = 0;

    private void Awake()
    {
        draggingStarted = false;
        direction = Direction.None;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingStarted = true;
        startPos = eventData.pressPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingStarted && direction != Direction.None)
        {
            //Debug.Log("swiped");
            //A swipe is detected
            if (direction == Direction.Left)
            {
                xManager = -1;
            }
            if (direction == Direction.Right)
            {
                xManager = 1;
            }
            if (direction == Direction.Up)
            {
                yManager = 1;
            }
            if (direction == Direction.Down)
            {
                yManager = -1;
            }
        }

        //reset the variables
        startPos = Vector2.zero;
        endPos = Vector2.zero;
        draggingStarted = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingStarted)
        {
            endPos = eventData.position;

            Vector2 difference = endPos - startPos; // difference vector between start and end positions.

            if (difference.magnitude > swipeThreshold)
            {
                if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y)) // Do horizontal swipe
                {
                    direction = difference.x > 0 ? Direction.Right : Direction.Left; // If greater than zero, then swipe to right.
                }
                else //Do vertical swipe
                {
                    direction = difference.y > 0 ? Direction.Up : Direction.Down; // If greater than zero, then swipe to up.
                }
            }
            else
            {
                direction = Direction.None;
            }
        }
    }
}