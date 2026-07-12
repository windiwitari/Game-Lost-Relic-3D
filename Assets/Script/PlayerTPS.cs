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
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Mobile Controller (Joystick Pack)")]
    [SerializeField] private FixedJoystick mobileJoystick; 

    private bool isGrounded;

    private CharacterController controller;
    private TPS inputActions;

    private Vector2 moveInput;
    private bool jumpPressed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new TPS();
        Time.timeScale = 1f; // Paksa waktu berjalan normal
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
        // 1. Cek sphere bawaan via GroundCheck
        bool sphereCheck = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        // 2. Cek tambahan bawaan CharacterController agar tidak pernah meleset
        isGrounded = sphereCheck || controller.isGrounded;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpPressed = true;
    }

    public void TriggerMobileJump()
    {
        if (isGrounded)
        {
            jumpPressed = true; 
        }
    }

    private void HandleMovement()
    {
        Vector2 finalInput = moveInput;

        if (mobileJoystick != null)
        {
            Vector2 joystickValues = new Vector2(mobileJoystick.Horizontal, mobileJoystick.Vertical);
            if (joystickValues.sqrMagnitude > 0.01f)
            {
                finalInput = joystickValues;
            }
        }

        Vector3 move = new Vector3(finalInput.x, 0, finalInput.y);

        if (move.magnitude > 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;

            Vector3 moveDirection = camForward * move.z + camRight * move.x;

            if (moveDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

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

            if (SFXManager.instance != null)
            {
                SFXManager.instance.PlaySFX(SFXManager.instance.jumpSound);
            }
        }

        jumpPressed = false;
    }

    private void ApplyGravity()
    {
        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f; // Reset gravitasi saat menyentuh tanah agar tidak menumpuk
        }
        else
        {
            velocityY += gravity * Time.deltaTime;
        }

        // Terapkan gravitasi
        controller.Move(new Vector3(0, velocityY, 0) * Time.deltaTime);
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", velocityY);
    }
}