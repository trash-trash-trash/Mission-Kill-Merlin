using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public PlayerInputHandler playerInputs;

    public Transform cameraArm;
    public float mouseSensitivity = 1.5f;

    public bool canLook = true;

    //hack
    public Transform cameraTransform;
    public Transform bodyTransform;

    public float x = 0;
    public float y = 0;

    public float minYaw = -80f;
    public float maxYaw = 80f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    void Start()
    {
        SetLook(100, 10);
    }


    public void Awake()
    {
        playerInputs.AnnounceLookVector2 += HandleLook;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void Update()
    {
        cameraTransform.position = bodyTransform.position;
    }

    private void HandleLook(Vector2 lookInput)
    {
        if (!canLook) return;

        y += lookInput.x * mouseSensitivity * Time.deltaTime;
        x -= lookInput.y * mouseSensitivity * Time.deltaTime;

        x = Mathf.Clamp(x, minPitch, maxPitch);
        y = Mathf.Clamp(y, minYaw, maxYaw);

        ApplyRotation();
    }

    public void SetLook(float newYaw, float newPitch)
    {
        y = Mathf.Clamp(newYaw, minYaw, maxYaw);
        x = Mathf.Clamp(newPitch, minPitch, maxPitch);

        ApplyRotation();
    }

    private void ApplyRotation()
    {
        cameraArm.rotation = Quaternion.Euler(0, -y,x);
    }

    void OnDisable()
    {
        playerInputs.AnnounceLookVector2 -= HandleLook;
    }
}