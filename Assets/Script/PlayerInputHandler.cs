using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string rotateObject = "RotateObject";
    [SerializeField] private string sprint = "Sprint";

    [Header("Sistem Kendali Mobile (Joystick Pack)")]
    [SerializeField] private FixedJoystick mobileJoystick; // Tambahan untuk menyambungkan UI Joystick

    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction rotateObjectAction;
    private InputAction sprintAction;

    // Menyimpan nilai murni dari Keyboard/Gamepad
    private Vector2 rawMovementInput;

    // KUNCI UTAMA: Menggabungkan nilai input keyboard/device dengan geseran analog joystick secara real-time
    public Vector2 MovementInput 
    { 
        get 
        {
            if (mobileJoystick != null)
            {
                // Ambil nilai horizontal dan vertical dari Joystick Pack
                Vector2 joystickValues = new Vector2(mobileJoystick.Horizontal, mobileJoystick.Vertical);
                
                // Jika joystick sedang digeser, prioritaskan nilai joystick. Jika tidak, pakai nilai keyboard/raw
                if (joystickValues.sqrMagnitude > 0.01f)
                {
                    return joystickValues;
                }
            }
            return rawMovementInput;
        }
    }

    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool RotateObjectTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        rotateObjectAction = mapReference.FindAction(rotateObject);
        sprintAction = mapReference.FindAction(sprint);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        // Mengalihkan pembacaan movement asli ke variabel rawMovementInput
        movementAction.performed += inputInfo => rawMovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => rawMovementInput = Vector2.zero;

        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;

        sprintAction.performed += inputInfo => SprintTriggered = true;
        sprintAction.canceled += inputInfo => SprintTriggered = false;

        rotateObjectAction.performed += _ => RotateObjectTriggered = true;
        rotateObjectAction.canceled += _ => RotateObjectTriggered = false;
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }
}