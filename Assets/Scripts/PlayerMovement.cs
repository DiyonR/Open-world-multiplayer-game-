using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float dodgeDistance = 5f;
    public float dodgeDuration = 0.5f;

    private CharacterController controller;
    Animator animator;
    private Vector3 velocity;
    private bool isDodging = false;
    private Vector3 dodgeDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Player Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        Vector3 moveVelocity = moveDirection * moveSpeed;

        // Apply gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;

        // Apply movement
        controller.Move(moveVelocity * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);

        // Jump
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Dodge
        if (!isDodging && Input.GetKeyDown(KeyCode.LeftShift))
            Dodge();

    }

    void Dodge()
    {
        // Determine dodge direction based on current movement direction
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        dodgeDirection = moveDirection * dodgeDistance;

        // Start dodging
        isDodging = true;
        Invoke("EndDodge", dodgeDuration);
    }

    void EndDodge()
    {
        isDodging = false;
        dodgeDirection = Vector3.zero; // Reset dodge direction
    }

    void FixedUpdate()
    {
        // Apply dodge movement
        if (isDodging)
            controller.Move(dodgeDirection * Time.deltaTime);
    }
}
