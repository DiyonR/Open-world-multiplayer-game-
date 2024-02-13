using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float dodgeDistance = 5f;
    public float dodgeLerpTime = 0.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isDodging = false;
    private Vector3 dodgeDirection;
    private Animator animator; // Reference to the Animator component

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Initialize the Animator component
    }

    private void Update()
    {
        // Player Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        Vector3 moveVelocity = moveDirection * moveSpeed;

        // Check if grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensure gravity doesn't accumulate while grounded
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Apply movement
        controller.Move(moveVelocity * Time.deltaTime);

        // Jump
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Dodge
        if (!isDodging && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dodge();
        }

        // Apply dodge movement
        if (isDodging)
        {
            controller.Move(dodgeDirection * Time.deltaTime / dodgeLerpTime);

            // End dodge after dodgeLerpTime
            Invoke("EndDodge", dodgeLerpTime);
        }
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Trigger jump animation
        animator.SetTrigger("Jump");
    }

    private void Dodge()
    {
        // Determine dodge direction based on input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Normalize the input vector to ensure consistent movement speed in all directions
        Vector3 dodgeDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (dodgeDirection != Vector3.zero)
        {
            // Calculate the angle of the dodge direction relative to the player's forward direction
            float angle = Vector3.SignedAngle(Vector3.forward, dodgeDirection, Vector3.up);

            // Determine the dodge animation based on the angle
            string animationTrigger = "";
            if (angle < -45f)
                animationTrigger = "RollLeft";
            else if (angle > 45f)
                animationTrigger = "RollRight";
            else if (angle < 0f)
                animationTrigger = "RollBackward";
            else
                animationTrigger = "RollForward";

            // Trigger the appropriate dodge animation
            animator.SetTrigger(animationTrigger);
        }

        isDodging = true;
    }

    private void EndDodge()
    {
        isDodging = false;
    }
}
