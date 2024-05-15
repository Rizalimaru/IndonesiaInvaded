using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public static PlayerAnimator instance;
    public Animator anim;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    Rigidbody rb;

    [Header("Movement")]
    public float acceleration =10f; // Acceleration rate
    public float maxMovement = 1.5f; // Maximum movement value
    float currentMovement = 0f; // Current movement value

    // New variables for tracking movement changes
    bool wasMoving = false;
    bool isStopping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        // Update animator parameters based on movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && currentMovement > 0; // Check if player is sprinting

        if (isSprinting)
        {   
            currentMovement = Mathf.MoveTowards(currentMovement, maxMovement, acceleration * Time.deltaTime); // Increase movement gradually to maxMovement when sprinting
        }
        else
        {
            currentMovement = Mathf.MoveTowards(currentMovement, Mathf.Max(Mathf.Abs(horizontalInput), Mathf.Abs(verticalInput)), acceleration * Time.deltaTime); // Increase movement gradually based on input
        }

        if(currentMovement > 1)
        {
            anim.SetTrigger("idleToSprint");
        }

        // Set animator parameters
        anim.SetFloat("movement", currentMovement); // Set movement parameter for blend tree
        anim.SetBool("isSprint", isSprinting); // Set isSprint parameter
        anim.SetBool("isRun", currentMovement == 1);
        anim.SetBool("isIdle", currentMovement == 0);

        // Check if we're stopping
        bool isMoving = currentMovement > 1;
        if (wasMoving && !isMoving)
        {
            isStopping = true;
        }
        else
        {
            isStopping = false;
        }
        anim.SetBool("isStopping", isStopping);

        // Update wasMoving for the next frame
        wasMoving = isMoving;

        // Check if we're jumping
        if (!grounded)
        {
            // Ensure the jump animation is playing
            anim.SetBool("isJump", true);
        }
        else
        {
            // If grounded, ensure the jump animation is not playing
            anim.SetBool("isJump", false);
        }
    }
}
