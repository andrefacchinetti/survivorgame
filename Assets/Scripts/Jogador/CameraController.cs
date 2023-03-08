using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform playerBody;

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;

    private float rotY = 0.0f; // rotação em Y
    private float rotX = 0.0f; // rotação em X
    private float smoothRotY = 0.0f; // rotação suavizada em Y
    private float smoothRotX = 0.0f; // rotação suavizada em X
    private float smoothVelocityY = 0.0f; // velocidade suavizada em Y
    private float smoothVelocityX = 0.0f; // velocidade suavizada em X
    private float smoothDampTime = 0.1f; // tempo de interpolação suave
    private Vector3 offset; // distância entre a câmera e o personagem
    private Vector3 smoothOffset; // distância suavizada entre a câmera e o personagem

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotY = angles.y;
        rotX = angles.x;

        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // Obter o movimento do mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calcular a rotação do personagem
        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX -= mouseY * mouseSensitivity * Time.deltaTime;

        // Limitar a rotação em X
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        // Suavizar a rotação
        smoothRotY = Mathf.SmoothDamp(smoothRotY, rotY, ref smoothVelocityY, smoothDampTime);
        smoothRotX = Mathf.SmoothDamp(smoothRotX, rotX, ref smoothVelocityX, smoothDampTime);

        // Converter as rotações em uma rotação Quaternion
        Quaternion localRotation = Quaternion.Euler(smoothRotX, smoothRotY, 0.0f);

        // Calcular a posição da câmera
        Vector3 targetPosition = target.position + offset;
        RaycastHit hit;
        if (Physics.Linecast(target.position, targetPosition, out hit))
        {
            smoothOffset = Vector3.Lerp(smoothOffset, offset.normalized * (hit.distance - 0.2f), Time.deltaTime * 10.0f);
        }
        else
        {
            smoothOffset = Vector3.Lerp(smoothOffset, offset, Time.deltaTime * 10.0f);
        }
        Vector3 finalPosition = target.position + localRotation * smoothOffset;

        // Atualizar a posição e rotação da câmera
        transform.position = finalPosition;
        playerBody.transform.rotation = localRotation;

    }
}
