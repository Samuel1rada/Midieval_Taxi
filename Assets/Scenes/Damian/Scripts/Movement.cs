using UnityEngine;
using UnityEngine.InputSystem;

public class HorseController : MonoBehaviour
{
    public float walkSpeed = 5f; // Speed for forward movement
    public float reverseSpeed = 3f; // Speed for backward movement
    public float turnSpeed = 100f; // Speed for turning
    public float stopForce = 10f; // Force to stop the horse

    private Rigidbody rb;
    private bool isDriving; // True when the "Drive" action is performed
    private bool isBraking; // True when the "Brake" action is performed
    private float turnInput; // Stores the XAxis input for turning

    private InputActions inputActions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);

        inputActions = new InputActions();

        // Bind input actions
        inputActions.Player.Drive.performed += ctx => isDriving = true;
        inputActions.Player.Drive.canceled += ctx => isDriving = false;

        inputActions.Player.Brake.performed += ctx => isBraking = true;
        inputActions.Player.Brake.canceled += ctx => isBraking = false;

        inputActions.Player.XAxis.performed += ctx => turnInput = ctx.ReadValue<Vector2>().x;
        inputActions.Player.XAxis.canceled += ctx => turnInput = 0f;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void FixedUpdate()
    {
        HandleMovement();
        HandleTurning();
        HandleBraking();
    }

    private void HandleMovement()
    {
        if (isDriving)
        {
            // Move forward
            Vector3 movement = transform.forward * walkSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
        else if (isBraking && rb.linearVelocity.magnitude < 0.1f) // Only move backward if the horse is nearly stopped
        {
            // Move backward
            Vector3 movement = -transform.forward * reverseSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }

    private void HandleTurning()
    {
        if (turnInput != 0)
        {
            // Rotate the horse based on the XAxis input
            float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0));
        }
    }

    private void HandleBraking()
    {
        if (!isDriving && !isBraking)
        {
            // Gradually stop the horse if no input is given
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, stopForce * Time.fixedDeltaTime);
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, stopForce * Time.fixedDeltaTime);
        }
    }
}