using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mouse : MonoBehaviour
{
    public float mouseSensitivity = 10f;

    public Transform cuerpoJugador;

    float rotaX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotaX -= mouseY;
        rotaX = Mathf.Clamp(rotaX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotaX, 0f, 0f);

        cuerpoJugador.rotation *= Quaternion.Euler(0f, mouseX, 0f);
    }
}