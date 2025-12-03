using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    [Header("Player Setup")]
    public Transform player;

    [Header("View Point Setup")]
    public Transform viewPoint;
    public Transform AIMViewPoint;

    [Header("Rotation Settings")]
    public float rotationSpeed;

    [Header("Camera Setup")]
    public GameObject TPSCamera;
    public GameObject AIMCamera;
    public GameObject crosshair;
    bool TPSMode = true, AIMMode = false;

    public void CameraModeChanger(bool TPS, bool AIM)
    {
        TPSMode = TPS;
        AIMMode = AIM;

        if (TPS) {
            TPSCamera.SetActive(true);
            AIMCamera.SetActive(false);
            crosshair.SetActive(false);
        } else if (AIM) {
            TPSCamera.SetActive(false);
            AIMCamera.SetActive(true);
            crosshair.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        crosshair.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        viewPoint.forward = viewDir.normalized;

        if (TPSMode) {
            Vector3 InputDir = viewPoint.forward * verticalInput + viewPoint.right * horizontalInput;
            if (InputDir != Vector3.zero) {
                player.forward = Vector3.Slerp(player.forward, InputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        } else if (AIMMode) {
            Vector3 dirToCombatLookAt = AIMViewPoint.position - new Vector3(transform.position.x, AIMViewPoint.position.y, transform.position.z);

            AIMViewPoint.forward = dirToCombatLookAt.normalized;

            player.forward = Vector3.Slerp(player.forward, dirToCombatLookAt.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
