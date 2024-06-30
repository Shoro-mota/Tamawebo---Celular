using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Asigna la cámara en el inspector
    public float followSpeed = 2.0f;  // Velocidad de seguimiento
    public Vector3 offset = new Vector3(0, 0, 0);  // Offset para ajustar la posición del personaje respecto a la cámara

    void Update()
    {
        if (cameraTransform != null)
        {
            Vector3 targetPosition = cameraTransform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}