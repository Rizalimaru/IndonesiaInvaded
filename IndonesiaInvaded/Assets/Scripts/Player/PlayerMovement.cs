using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{   
    private Animator animator;
    public static PlayerMovement instance;
    
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Dodge")]
    [SerializeField] AnimationCurve dodgeCurve;
    bool isDodging;
    float dodgeTimer;
    public KeyCode dodgeKey = KeyCode.LeftControl;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;
    public Transform orientationForAtk;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    [Header("Gravity")]
    public float gravity = 9.81f; // Default gravity value
    
    private void Start()
    {   
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;

        Keyframe dodge_lastFrame = dodgeCurve[dodgeCurve.length - 1];
        dodgeTimer = dodge_lastFrame.time;
    }
    
    private void Update()
    {   
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        if(InputManager.instance.GetExitPressed())
        {
            GameManager.instance.SaveGame();
            SceneManager.LoadSceneAsync("MainMenu");
        }

        if (grounded)
        {
            animator.SetBool("isGrounded", true);
        }else
        {
            animator.SetBool("isGrounded", false);
        }
    }

    private void FixedUpdate()
    {   
        if(animator.GetBool("hit1") || animator.GetBool("hit2") || animator.GetBool("hit3"))
        {
            MoveForwardWhileAtk();
        }else
        {
            MovePlayer();
        }
        
    }


    private void MyInput()
    {   

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

         // start dodge
        if(Input.GetKeyDown(dodgeKey))
        {
            if(moveDirection.magnitude !=0)
            {
                StartCoroutine(Dodge());
            }
        }
    }

    IEnumerator Dodge()
    {
        isDodging = true;
        float timer = 0;
        animator.SetTrigger("Dodge");
        while(timer < dodgeTimer)
        {
            float speed = dodgeCurve.Evaluate(timer);
            Vector3 dir = (transform.forward * speed) + (Vector3.up * rb.velocity.y); // Menggunakan rb.velocity
            // Mengganti pemanggilan CharacterController.Move() dengan pemanggilan transform.Translate() atau rb.MovePosition() jika tidak menggunakan CharacterController
            transform.Translate(dir * Time.deltaTime); // atau rb.MovePosition(transform.position + dir * Time.deltaTime) jika tidak menggunakan CharacterController
            timer += Time.deltaTime;
            yield return null;
        }
        isDodging = false;
    }

    private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {   
        // Check if any hit animation is active
        bool hit1 = animator.GetBool("hit1");
        bool hit2 = animator.GetBool("hit2");
        bool hit3 = animator.GetBool("hit3");
        bool RoarSkill = animator.GetBool("RoarSkill");

        if(hit1)
        {
            return;
        }

        if(isDodging)
        {
            return;
        }
    
        // Stop player movement if hit animation is active
        if (RoarSkill)
        {
            StopMovement(); // Stop player movement
            return; // Exit the method early
        }

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // Apply gravity
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    private void MoveForwardWhileAtk()
    {   
        // Mengecek apakah sedang melakukan animasi hit1, hit2, atau hit3
        bool hit1 = animator.GetBool("hit1");
        bool hit2 = animator.GetBool("hit2");
        bool hit3 = animator.GetBool("hit3");
        bool hit4 = animator.GetBool("hit4");

        // Mengatur kecepatan berdasarkan animasi yang sedang aktif
        float forwardSpeed = 0f;
        if(hit1)
        {
            forwardSpeed = 1f;
        }
        else if(hit2)
        {
            forwardSpeed = 1f;
        }
        else if(hit3)
        {
            forwardSpeed = 2f;
        }
        else if(hit4)
        {
            forwardSpeed = 1f;
        }

        // Hanya melakukan pergerakan maju jika karakter berada di tanah dan tidak ada input dari pengguna
        if(grounded)
        {
            // Menentukan arah gerakan berdasarkan orientasi karakter
            moveDirection = orientationForAtk.forward * verticalInput + orientationForAtk.right * horizontalInput;

            // Jika sedang menyerang dan tidak ada input vertikal, tetapkan kecepatan maju
            if ((hit1 || hit2 || hit3 || hit4) && verticalInput == 0)
            {
                moveDirection += orientationForAtk.forward;
            }

            // Menambahkan gaya untuk bergerak maju dengan kecepatan sesuai animasi serangan
            Vector3 targetVelocity = moveDirection.normalized * forwardSpeed * 2f;
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.deltaTime * 10f); // Menggunakan lerp untuk menginterpolasi kecepatan
        }
    }


    public void StopMovement()
    {
        rb.velocity = Vector3.zero; // Mengatur kecepatan pemain menjadi nol
        moveDirection = Vector3.zero; // Mengatur arah gerakan pemain menjadi nol
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }


    
}
