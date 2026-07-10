using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerTPS : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
    private float velocityY;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private LayerMask groundMask;

    [Header("Mobile Controller (Joystick Pack)")]
    [SerializeField] private FixedJoystick mobileJoystick; // Kolom baru untuk drag joystick dari Canvas

    private bool isGrounded;

    private CharacterController controller;
    private TPS inputActions;

    private Vector2 moveInput;
    private bool jumpPressed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new TPS();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Jump.performed -= OnJump;

        inputActions.Disable();
    }

    private void Update()
    {
        CheckGround();
        HandleMovement();
        HandleJump();
        ApplyGravity();
        UpdateAnimator();
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpPressed = true;
    }

    private void HandleMovement()
    {
        // 1. Ambil input dasar dari New Input System (Keyboard / Gamepad)
        Vector2 finalInput = moveInput;

        // 2. KUNCI UTAMA: Jika Joystick di layar disentuh/digerakkan, gabungkan nilainya ke sistem pergerakan
        if (mobileJoystick != null)
        {
            Vector2 joystickValues = new Vector2(mobileJoystick.Horizontal, mobileJoystick.Vertical);
            
            // Jika analog digeser melebihi batas toleransi kecil, override nilai inputnya
            if (joystickValues.sqrMagnitude > 0.01f)
            {
                finalInput = joystickValues;
            }
        }

        // Konversi nilai input gabungan ke Vector3 gerakan dunia game
        Vector3 move = new Vector3(finalInput.x, 0, finalInput.y);

        if (move.magnitude > 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;

            Vector3 moveDirection = camForward * move.z + camRight * move.x;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            controller.Move(moveDirection.normalized * moveSpeed * Time.deltaTime);

            animator.SetBool("isWalk", true);
        }
        else
        {
            animator.SetBool("isWalk", false);
        }
    }

    private void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            velocityY = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger("jump");
        }

        jumpPressed = false;
    }

    private void ApplyGravity()
    {
        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f;
        }

        velocityY += gravity * Time.deltaTime;

        Vector3 gravityMove = new Vector3(0, velocityY, 0);
        controller.Move(gravityMove * Time.deltaTime);
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", velocityY);
    }
}