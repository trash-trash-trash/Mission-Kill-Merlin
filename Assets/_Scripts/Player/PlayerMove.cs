using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public PlayerInputHandler inputHandler;
    [SerializeField] private float moveSpeed = 5f;

    public Rigidbody rb;
    private Vector2 inputDirection;

    public Transform cameraArmTransform;

    public bool canMove = true;

    public event Action<Vector3> AnnounceMoveVector;

    private void OnEnable()
    {
        inputHandler.AnnounceMoveVector2 += OnMoveInput;
    }

    private void OnDisable()
    {
        inputHandler.AnnounceMoveVector2 -= OnMoveInput;
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
}