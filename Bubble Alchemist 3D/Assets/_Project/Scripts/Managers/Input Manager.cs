using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private Vector2 movement;
    [SerializeField] private bool wasJumpPressed;
    [SerializeField] private bool wasJumpTouched;
    [SerializeField] private bool isJumpHeld;
    [SerializeField] private bool wasJumpReleased;
    [SerializeField] private bool isRunHeld;

    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction runAction;

    public Vector2 GetMovement() { return movement; }
    public bool WasJumpPressed() { return wasJumpPressed; }
    public bool WasJumpTouched() { return wasJumpTouched; }
    public bool IsJumpHeld() { return isJumpHeld; }
    public bool WasJumpReleased() { return wasJumpReleased;  }
    public bool IsRunHeld() { return isRunHeld;  }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this) ; else instance = this;

        //playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        runAction = playerInput.actions["Sprint"];
    }

    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        //movement = new Vector2(Input.GetAxis("Horizontal"), 0);

        wasJumpPressed = jumpAction.WasPressedThisFrame();
        isJumpHeld = jumpAction.IsPressed();
        wasJumpReleased = jumpAction.WasReleasedThisFrame();

        isRunHeld = runAction.IsPressed();
    }
}
