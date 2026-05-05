using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public PlayerInputHandler inputHandler;
    [SerializeField] private float moveSpeed = 5f;

    public Rigidbody rb;
    private Vector2 inputDirection;

    public Transform cameraArmTransform;

    public bool canMove = true;

    public event Action<Vector3> AnnounceMoveVector;

    [Header("DASH")] public float dashSpeed = 10f;
    public float dashTime = 0.5f;
    public float dashCooldown = 0.5f;
    public float timeTilNextDash = 0;
    public bool canDash = true;
    public bool dashing = false;

    public event Action<bool> AnnounceDash;

    private void OnEnable()
    {
        inputHandler.AnnounceMoveVector2 += OnMoveInput;
        inputHandler.AnnounceSpaceBar += OnDashInput;
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;
        //had to flip x/y and max x negative for some reason...
        Vector3 localMove = new Vector3(inputDirection.y, 0, -inputDirection.x);
        
        //changes direction according to eye's transform rotation
        Vector3 worldMove = new Vector3(cameraArmTransform.TransformDirection(localMove).x * moveSpeed,
            cameraArmTransform.TransformDirection(localMove).y * moveSpeed,cameraArmTransform.TransformDirection(localMove).z * moveSpeed);
        
        rb.linearVelocity = new Vector3(worldMove.x, rb.linearVelocity.y, worldMove.z);
        
        AnnounceMoveVector?.Invoke(worldMove);
    }

    private void OnMoveInput(Vector2 direction)
    {
        inputDirection = direction;
    }

    void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (dashing || !canDash)
                return;
            
            StartCoroutine(StartDash(inputDirection));
        }
    }

    IEnumerator StartDash(Vector2 direction)
    {
        AnnounceDash?.Invoke(true);
        
        canMove = false;
        canDash = false;
        dashing = true;

        Vector3 dashDirection;

        if (direction == Vector2.zero)
        {
            //if player input is neutral dash backwards relative to facing direction
            dashDirection = -rb.transform.forward;
        }
        else
        {
            Vector3 localMove = new Vector3(direction.y, 0, -direction.x);
            dashDirection = cameraArmTransform.TransformDirection(localMove).normalized;
        }

        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            rb.linearVelocity = new Vector3(
                dashDirection.x * dashSpeed,
                rb.linearVelocity.y,
                dashDirection.z * dashSpeed
            );

            yield return null;
        }

        dashing = false;
        canMove = true;

        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        timeTilNextDash = dashCooldown;

        while (timeTilNextDash > 0)
        {
            timeTilNextDash -= Time.deltaTime;
            yield return null;
        }

        timeTilNextDash = 0;
        canDash = true;
        
        AnnounceDash?.Invoke(false);
    }

    private void OnDisable()
    {
        inputHandler.AnnounceMoveVector2 -= OnMoveInput;
        inputHandler.AnnounceSpaceBar -= OnDashInput;
    }
}