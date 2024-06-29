using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCamera : MonoBehaviour
{
    public float swipeSpeed = 0.1f;
    private Vector2 startTouchPosition, endTouchPosition;
    private bool isSwiping = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isSwiping = true;
                    break;

                case TouchPhase.Moved:
                    if (isSwiping)
                    {
                        endTouchPosition = touch.position;
                        Vector2 swipeDirection = endTouchPosition - startTouchPosition;

                        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                        {
                            // Horizontal swipe
                            if (swipeDirection.x > 0)
                            {
                                // Swipe to the right
                                transform.Translate(Vector3.right * swipeSpeed * Time.deltaTime);
                            }
                            else
                            {
                                // Swipe to the left
                                transform.Translate(Vector3.left * swipeSpeed * Time.deltaTime);
                            }
                        }
                        startTouchPosition = endTouchPosition;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isSwiping = false;
                    break;
            }
        }
    }
}
