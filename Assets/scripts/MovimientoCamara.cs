using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCamera : MonoBehaviour
{
    public float swipeSpeed = 0.1f;
    private Vector2 startTouchPosition, endTouchPosition;
    private bool isSwiping = false;

    // Límites de movimiento de la cámara
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

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
                                MoveCamera(Vector3.right);
                            }
                            else
                            {
                                // Swipe to the left
                                MoveCamera(Vector3.left);
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

    void MoveCamera(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction * swipeSpeed * Time.deltaTime;

        // Aplicar límites de movimiento
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;
    }
}