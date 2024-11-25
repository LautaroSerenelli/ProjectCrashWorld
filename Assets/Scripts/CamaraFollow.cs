using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // El objeto que la cámara seguirá
    public float distance = 12f; // Distancia inicial entre la cámara y el objetivo
    public float sensitivity = 2f; // Sensibilidad del movimiento del ratón
    public float upVerticalLimit = 45f; // Límite de rotación vertical hacia arriba
    public float downVerticalLimit = 0f; // Límite de rotación vertical hacia abajo
    public float maxUpDistance = 20f; // Distancia máxima de la cámara cuando mira hacia arriba
    public float yOffset = 2f; // Desplazamiento hacia arriba de la posición de la cámara

    private float mouseX = 0f; // Movimiento del ratón en el eje X
    private float mouseY = 0f; // Movimiento del ratón en el eje Y

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro de la pantalla
        Cursor.visible = false; // Hacer que el cursor sea invisible
    }

    void LateUpdate()
    {
        // Obtener el movimiento del ratón
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * sensitivity;

        // Limitar el movimiento vertical del ratón
        float verticalLimit = mouseY > 0 ? upVerticalLimit : downVerticalLimit;
        mouseY = Mathf.Clamp(mouseY, -verticalLimit, verticalLimit);

        // Ajustar la distancia de la cámara según la rotación vertical
        float adjustedDistance = Mathf.Lerp(distance, maxUpDistance, Mathf.Abs(mouseY / upVerticalLimit));

        // Calcular la rotación de la cámara
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0f);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -adjustedDistance);
        Vector3 position = rotation * negDistance + target.position + Vector3.up * yOffset; // Ajustar la posición de la cámara hacia arriba

        // Actualizar la posición y rotación de la cámara
        transform.rotation = rotation;
        transform.position = position;
    }
}