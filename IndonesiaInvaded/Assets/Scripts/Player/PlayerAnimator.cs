using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{   
    private PlayerMovement playerMovement;
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
    float currentMovementZ = 0f; // Current movement value
    float currentMovementX = 0f; // Current movement value
    float velocityX = 0.0f; // Current velocity on X axis
    float velocityZ = 0.0f; // Current velocity on Z axis
    // New variables for tracking movement changes
    private bool hasExecute = false;
    private KeyCode rangedAttackKey = KeyCode.Mouse1;

    private void Start()
    {   
        playerMovement = FindAnyObjectByType<PlayerMovement>();
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
        float currentValue = playerMovement.magnitude;
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround | whatIsGround2);

        //TwoDimentionalMovement();
        UpdateAnimator();

        if (currentValue == 0f && !hasExecute)
        {
            // Panggil fungsi untuk men-trigger animasi stop
            TriggerStopAnimation();

            // Set variabel hasExecuted menjadi true agar tidak dieksekusi lagi
            hasExecute = true;
        }
        else if (currentValue != 0f)
        {
            // Reset hasExecuted jika nilai float tidak sama dengan 0
            hasExecute = false;
        }
    }

#region TwoDimentionalMovement (Not Use)
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
#endregion

    void TriggerStopAnimation()
    {
        // Men-trigger animasi stop dengan parameter trigger
        anim.SetTrigger("Stopping");
    }
    private void UpdateAnimator()
    {
        // Update animator parameters based on movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        bool verticalUp = Input.GetKey(KeyCode.W);
        bool verticalDown = Input.GetKey(KeyCode.S);
        bool horizontalLeft = Input.GetKey(KeyCode.A);
        bool horizontalRight = Input.GetKey(KeyCode.D);

        bool ModeRange = Input.GetKey(rangedAttackKey);

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && currentMovementZ > 0; // Check if player is sprinting

        if (!ModeRange)
        {   
            anim.SetBool("RangeAtkAktif", false);
            if (isSprinting)
            {   
                currentMovementZ = Mathf.MoveTowards(currentMovementZ, maxMovement, acceleration * Time.deltaTime); // Increase movement gradually to maxMovement when sprinting
            }
            else
            {
                currentMovementZ = Mathf.MoveTowards(currentMovementZ, Mathf.Max(Mathf.Abs(horizontalInput), Mathf.Abs(verticalInput)), acceleration * Time.deltaTime); // Increase movement gradually based on input
            }

            // Deceleration for stopping smoothly
            if (!verticalUp && !verticalDown)
            {
                currentMovementZ = Mathf.MoveTowards(currentMovementZ, 0, deceleration * Time.deltaTime);
            }
            if (!horizontalLeft && !horizontalRight)
            {
                currentMovementX = Mathf.MoveTowards(currentMovementX, 0, deceleration * Time.deltaTime);
            }
        }
        else if (ModeRange)
        {   
            anim.SetBool("RangeAtkAktif", true);
            if (verticalUp && currentMovementZ < 1)
            {
                currentMovementZ += acceleration * Time.deltaTime;
            }
            if (verticalDown && currentMovementZ > -1)
            {
                currentMovementZ -= acceleration * Time.deltaTime;
            }
            if (horizontalLeft && currentMovementX > -1)
            {
                currentMovementX -= acceleration * Time.deltaTime;
            }
            if (horizontalRight && currentMovementX < 1)
            {
                currentMovementX += acceleration * Time.deltaTime;
            }
            if (!verticalUp && !verticalDown)
            {
                currentMovementZ = Mathf.MoveTowards(currentMovementZ, 0, deceleration * Time.deltaTime);
            }
            if (!horizontalLeft && !horizontalRight)
            {
                currentMovementX = Mathf.MoveTowards(currentMovementX, 0, deceleration * Time.deltaTime);
            }
        }

        // Set animator parameters
        anim.SetFloat("movementZ", currentMovementZ); // Set movement parameter for blend tree
        anim.SetFloat("movementX", currentMovementX); // Set movement parameter for blend tree
        anim.SetBool("isSprint", isSprinting); // Set isSprint parameter
        anim.SetBool("isRun", currentMovementZ == 1);
        anim.SetBool("isIdle", currentMovementZ == 0);

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

        if(playerMovement.IsDodging == true)
        {
            anim.SetBool("isDodge", true);
        }
        else
        {
            anim.SetBool("isDodge", false);
        }
    }
}

