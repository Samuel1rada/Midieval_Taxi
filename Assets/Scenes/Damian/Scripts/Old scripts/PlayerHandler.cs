using UnityEngine;
using UnityEngine.InputSystem;

public class HorseMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 10f; // Maximum normal speed
    public float acceleration = 20f; // How fast the object accelerates
    public float turnSpeed = 5f; // How fast the object turns

    [Header("Leg Settings")]
    public Rigidbody frontLeftLeg; // Reference to the front left leg
    public Rigidbody frontRightLeg; // Reference to the front right leg
    public Rigidbody backLeftLeg; // Reference to the back left leg
    public Rigidbody backRightLeg; // Reference to the back right leg
    public float legSwingSpeed = 10f; // Speed of leg movement
    public float legSwingAngle = 45f; // Maximum swing angle for legs

    [Header("Ground Check Settings")]
    public float groundCheckDistance = 0.1f; // Distance to check for ground
    public LayerMask groundLayer; // Layer mask for ground objects

    private float currentSpeed = 0f;
    private bool isGrounded = false;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction xAxisAction;
    private InputAction driveAction;

    private Vector2 movementInput; // Stores the XAxis input (Vector2)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing!");
        }

        // Set up input system
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing!");
        }

        xAxisAction = playerInput.actions["XAxis"];
        driveAction = playerInput.actions["Drive"];
    }

    void Update()
    {
        CheckGrounded(); // Check if the player is grounded
    }

    void FixedUpdate()
    {
        if (isGrounded) // Only allow movement if grounded
        {
            HandleMovement();
            HandleTurning();
            HandleLegMovement(); // Move the legs only when moving
        }
        else
        {
            // If not grounded, slow down naturally
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, 10f * Time.fixedDeltaTime);
            rb.linearVelocity = transform.forward * currentSpeed;
        }
    }

    void HandleMovement()
    {
        float driveInput = driveAction.ReadValue<float>(); // Drive input (forward/backward)

        // Determine the target speed based on drive input
        if (driveInput > 0)
        {
            // Accelerate forward
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed * driveInput, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // No drive input, slow down naturally
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, 10f * Time.fixedDeltaTime);
        }

        // Move the object forward or backward
        rb.linearVelocity = transform.forward * currentSpeed;
    }

    void HandleTurning()
    {
        movementInput = xAxisAction.ReadValue<Vector2>(); // Read Vector2 input for turning

        // Calculate the turn direction based on X-axis input
        float turnInput = movementInput.x;

        // Smoothly rotate the object
        Quaternion deltaRotation = Quaternion.Euler(0f, turnInput * turnSpeed * currentSpeed / maxSpeed, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    void HandleLegMovement()
    {
        // Only move legs if the horse is moving
        if (currentSpeed > 0.1f)
        {
            // Simulate horse-like leg movement by swinging the legs back and forth
            float angle = Mathf.Sin(Time.time * legSwingSpeed) * legSwingAngle * (currentSpeed / maxSpeed);

            // Front legs move in opposite directions
            frontLeftLeg.transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
            frontRightLeg.transform.localRotation = Quaternion.Euler(-angle, 0f, 0f);

            // Back legs move in opposite directions, offset by 180 degrees for a natural gait
            backLeftLeg.transform.localRotation = Quaternion.Euler(-angle, 0f, 0f);
            backRightLeg.transform.localRotation = Quaternion.Euler(angle, 0f, 0f);
        }
        else
        {
            // Reset leg rotation when not moving
            frontLeftLeg.transform.localRotation = Quaternion.identity;
            frontRightLeg.transform.localRotation = Quaternion.identity;
            backLeftLeg.transform.localRotation = Quaternion.identity;
            backRightLeg.transform.localRotation = Quaternion.identity;
        }
    }

    void CheckGrounded()
    {
        // Cast a ray downward to check for ground
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer);

        // Optional: Visualize the ground check ray in the editor
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }
}