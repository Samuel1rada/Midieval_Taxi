using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float boostSpeed = 15f;
    public float brakeForce = 10f;
    public float rotationSpeed = 5f;
    public float handbrakeTurnMultiplier = 2f;

    private Vector2 moveInput;
    private bool isRunning;
    private bool isBoosting;
    private bool isBraking;
    private bool isHandbraking;

    private CharacterController characterController;
    private InputActions inputActions; // Use the correct class name here

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputActions = new InputActions(); // Initialize the InputActions

        // Register input callbacks
        inputActions.Player.XAxis.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.XAxis.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Boost.performed += ctx => isRunning = true;
        inputActions.Player.Boost.canceled += ctx => isRunning = false;

        inputActions.Player.Boost.performed += ctx => isBoosting = true;
        inputActions.Player.Boost.canceled += ctx => isBoosting = false;

        inputActions.Player.Brake.performed += ctx => isBraking = true;
        inputActions.Player.Brake.canceled += ctx => isBraking = false;

        inputActions.Player.Handbrake.performed += ctx => isHandbraking = true;
        inputActions.Player.Handbrake.canceled += ctx => isHandbraking = false;
    }

    private void OnEnable()
    {
        inputActions.Enable(); // Enable the input actions
    }

    private void OnDisable()
    {
        inputActions.Disable(); // Disable the input actions
    }

    private void Update()
    {
        MoveHorse();
    }

    private void MoveHorse()
    {
        // Calculate movement direction
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y).normalized;

        // Apply speed based on input
        float speed = walkSpeed;
        if (isRunning) speed = runSpeed;
        if (isBoosting) speed = boostSpeed;

        // Apply brake force
        if (isBraking)
        {
            speed = Mathf.Max(0, speed - brakeForce * Time.deltaTime);
        }

        // Apply handbrake effect (sharp turn or skid)
        if (isHandbraking)
        {
            movement.x *= handbrakeTurnMultiplier; // Increase turning ability
            speed *= 0.5f; // Reduce speed while handbraking
        }

        // Move the horse
        movement *= speed * Time.deltaTime;
        characterController.Move(movement);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}