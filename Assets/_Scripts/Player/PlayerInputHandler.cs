using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private HGPlayerControls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;

    public event Action<Vector2> AnnounceLookVector2;
    public event Action<Vector2> AnnounceMoveVector2;

    public event Action<InputAction.CallbackContext> AnnounceControl;

    public event Action<InputAction.CallbackContext> AnnounceRightClick;
    public event Action<InputAction.CallbackContext> AnnounceLeftClick;
    public event Action<InputAction.CallbackContext> AnnounceInventory01;
    public event Action<InputAction.CallbackContext> AnnounceInventory02;
    public event Action<InputAction.CallbackContext> AnnounceNextInventory;
    public event Action<InputAction.CallbackContext> AnnounceToggleRun;

    public event Action<InputAction.CallbackContext> AnnounceQ;

    public event Action<InputAction.CallbackContext> AnnounceR;

    public event Action<InputAction.CallbackContext> AnnounceShift;

    public event Action<InputAction.CallbackContext> AnnounceSpaceBar;


    private void Awake()
    {
        controls = new HGPlayerControls();

        controls.InGameActionMap.Control.performed += OnControl;
        controls.InGameActionMap.Control.canceled += OnControl;

        controls.InGameActionMap.MoveAction.performed += OnMove;
        controls.InGameActionMap.MoveAction.canceled += OnMove;

        controls.InGameActionMap.LookAction.performed += OnLook;
        controls.InGameActionMap.LookAction.canceled += OnLook;

        controls.InGameActionMap.LeftClick.performed += OnLeftClick;
        controls.InGameActionMap.LeftClick.canceled += OnLeftClick;

        controls.InGameActionMap.RightClick.performed += OnRightClick;
        controls.InGameActionMap.RightClick.canceled += OnRightClick;

        controls.InGameActionMap.SelectInventory01.performed += OnInventory01;
        controls.InGameActionMap.SelectInventory01.canceled += OnInventory01;

        controls.InGameActionMap.SelectInventory02.performed += OnInventory02;
        controls.InGameActionMap.SelectInventory02.canceled += OnInventory02;

        controls.InGameActionMap.SpaceBar.performed += OnSpaceBar;
        controls.InGameActionMap.SpaceBar.canceled += OnSpaceBar;

        controls.InGameActionMap.NextInventory.performed += NextInventory;
        controls.InGameActionMap.NextInventory.canceled += NextInventory;

        controls.InGameActionMap.Shift.performed += OnShift;
        controls.InGameActionMap.Shift.canceled += OnShift;

        controls.InGameActionMap.ToggleRun.performed += OnToggleRun;
        controls.InGameActionMap.ToggleRun.canceled += OnToggleRun;

        controls.InGameActionMap.Q.performed += OnQ;
        controls.InGameActionMap.Q.canceled += OnQ;

        controls.InGameActionMap.R.performed += OnR;
        controls.InGameActionMap.R.canceled += OnR;
    }

    private void OnSpaceBar(InputAction.CallbackContext context)
    {
        AnnounceSpaceBar?.Invoke(context);
    }


    private void OnQ(InputAction.CallbackContext context)
    {
        AnnounceQ?.Invoke(context);
    }

    private void OnR(InputAction.CallbackContext context)
    {
        AnnounceR?.Invoke(context);
    }

    private void OnControl(InputAction.CallbackContext context)
    {
        AnnounceControl?.Invoke(context);
    }

    private void OnShift(InputAction.CallbackContext context)
    {
        AnnounceShift?.Invoke(context);
    }

    private void OnInventory01(InputAction.CallbackContext context)
    {
        AnnounceInventory01?.Invoke(context);
    }

    private void OnInventory02(InputAction.CallbackContext context)
    {
        AnnounceInventory02?.Invoke(context);
    }

    private void NextInventory(InputAction.CallbackContext context)
    {
        AnnounceNextInventory?.Invoke(context);
    }

    private void OnToggleRun(InputAction.CallbackContext context)
    {
        AnnounceToggleRun?.Invoke(context);
    }

    private void OnEnable()
    {
        controls.Enable();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleMove();
    }

    private void HandleLook()
    {
        AnnounceLookVector2?.Invoke(lookInput);
    }

    private void HandleMove()
    {
        Vector2 clampedMove = new Vector2(
            Mathf.Abs(moveInput.x) < 0.1f ? 0 : Mathf.Sign(moveInput.x),
            Mathf.Abs(moveInput.y) < 0.1f ? 0 : Mathf.Sign(moveInput.y)
        );
        moveInput = clampedMove;
        AnnounceMoveVector2?.Invoke(moveInput);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            moveInput = context.ReadValue<Vector2>();
        else
            moveInput = Vector2.zero;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed)
            lookInput = context.ReadValue<Vector2>();
        else
            lookInput = Vector2.zero;
    }

    private void OnLeftClick(InputAction.CallbackContext context)
    {
        AnnounceLeftClick?.Invoke(context);
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        AnnounceRightClick?.Invoke(context);
    }

    void OnDisable()
    {
        controls.Disable();

        controls.InGameActionMap.Control.performed -= OnControl;
        controls.InGameActionMap.Control.canceled -= OnControl;
        controls.InGameActionMap.MoveAction.performed -= OnMove;
        controls.InGameActionMap.MoveAction.canceled -= OnMove;
        controls.InGameActionMap.LookAction.performed -= OnLook;
        controls.InGameActionMap.LookAction.canceled -= OnLook;
        controls.InGameActionMap.LeftClick.performed -= OnLeftClick;
        controls.InGameActionMap.LeftClick.canceled -= OnLeftClick;
        controls.InGameActionMap.RightClick.performed -= OnRightClick;
        controls.InGameActionMap.RightClick.canceled -= OnRightClick;
        controls.InGameActionMap.SelectInventory01.performed -= OnInventory01;
        controls.InGameActionMap.SelectInventory01.canceled -= OnInventory01;
        controls.InGameActionMap.SelectInventory02.performed -= OnInventory02;
        controls.InGameActionMap.SelectInventory02.canceled -= OnInventory02;
        controls.InGameActionMap.Q.performed -= OnQ;
        controls.InGameActionMap.Q.canceled -= OnQ;
        controls.InGameActionMap.R.performed -= OnR;
        controls.InGameActionMap.R.canceled -= OnR;
    }
}