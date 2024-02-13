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

    private void Start()
    {
        controller = GetComponent<CharacterController>();
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

        // Apply movement
        controller.Move((velocity + moveVelocity) * Time.deltaTime);

        // Jump
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
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

    private void Dodge()
    {
        // Determine dodge direction based on input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        dodgeDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        dodgeDirection.y = 0f; // Dodge only in the horizontal plane
        dodgeDirection = dodgeDirection.normalized * dodgeDistance;

        isDodging = true;
    }

    private void EndDodge()
    {
        isDodging = false;
    }
}
