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
    public LayerMask whatIsGround2;
    bool grounded;

    Rigidbody rb;

    [Header("Movement")]
    public float acceleration = 2f; // Acceleration rate
    public float deceleration = 2f; // Deceleration rate
    public float maxMovement = 1.5f; // Maximum movement value
    float currentMovement = 0f; // Current movement value
    float velocityX = 0.0f; // Current velocity on X axis
    float velocityZ = 0.0f; // Current velocity on Z axis
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
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround | whatIsGround2);

        TwoDimentionalMovement();
    }

    void TwoDimentionalMovement()
    {
        bool forwardPress = Input.GetKey(KeyCode.W);
        bool leftPress = Input.GetKey(KeyCode.A);
        bool rightPress = Input.GetKey(KeyCode.D);
        bool backwardPress = Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        // Increase velocity based on input
        if ((forwardPress || leftPress || rightPress || backwardPress) && velocityZ < 0.5f && !runPressed)
        {
            velocityZ += acceleration * Time.deltaTime;
        }

        if (leftPress && velocityX > -0.5f && !runPressed)
        {
            velocityX -= acceleration * Time.deltaTime;
        }

        if (rightPress && velocityX < 0.5f && !runPressed)
        {
            velocityX += acceleration * Time.deltaTime;
        }

        // Deceleration for Z
        if (!forwardPress && velocityZ > 0.0f)
        {
            velocityZ -= deceleration * Time.deltaTime;
        }

        if (!forwardPress && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }

        // Deceleration for X
        if (!leftPress && velocityX < 0.0f)
        {
            velocityX += deceleration * Time.deltaTime;
        }

        if (!rightPress && velocityX > 0.0f)
        {
            velocityX -= deceleration * Time.deltaTime;
        }

        if (!leftPress && !rightPress && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        // Set animator parameters
        //anim.SetFloat("movementX", velocityX);
        anim.SetFloat("movementZ", velocityZ);

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
        anim.SetFloat("movementZ", currentMovement); // Set movement parameter for blend tree
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
