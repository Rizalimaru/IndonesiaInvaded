using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private Combat combat;
    public static PlayerMovement instance;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float rangedAtkMoveSpeed = 5f;
    public float sprintSpeed;
    public float groundDrag;
    private bool isStopping = false;

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
    KeyCode rangedAtkKey = KeyCode.Mouse1;

    [Header("KnockBack")]
    public float knockShield = 100f;
    public float knockBackForce;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsGround2;
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
        rangedAtk,
        crouching,
        air
    }

    [Header("Gravity")]
    public float gravity = 9.81f; // Default gravity value

    private void Start()
    {
        combat = Combat.instance;
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
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround2 | whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        if (InputManager.instance.GetExitPressed())
        {
            GameManager.instance.SaveGame();
            SceneManager.LoadSceneAsync("MainMenu");
        }

        if (grounded)
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
    }

    private void FixedUpdate()
    {   
        bool rangeAtkAktif = Input.GetKey(rangedAtkKey);
        if (animator.GetBool("hit1") || animator.GetBool("hit2") || animator.GetBool("hit3") && !rangeAtkAktif)
        {
            MoveForwardWhileAtk();
        }
        else
        {
            MovePlayer();
        }

        if (isDodging)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded && combat.isAttacking == false)
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
        if (Input.GetKeyDown(dodgeKey))
        {   
            StartCoroutine(Dodge());
        }
    }

    IEnumerator Dodge()
    {
        if (!isDodging)
        {
            if (moveDirection.magnitude != 0)
            {
                isDodging = true;
                animator.SetTrigger("Dodge");

                // Calculate dodge direction
                Vector3 dodgeDirection = orientationForAtk.forward * verticalInput + orientationForAtk.right * horizontalInput;
                Vector3 targetVelocity = dodgeDirection.normalized * moveSpeed * 5f;

                // Apply dodge velocity
                rb.velocity = targetVelocity;

                // Disable collisions temporarily to prevent getting hit during dodge
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

                // Wait for the dodge duration
                yield return new WaitForSeconds(dodgeTimer);

                // Enable collisions after dodge
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

                isDodging = false;
            }
        }
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
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Ranged Attack
        else if (grounded && Input.GetKey(rangedAtkKey))
        {
            state = MovementState.rangedAtk;
            moveSpeed = rangedAtkMoveSpeed;
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

    #region basic player movement
    private void MovePlayer()
    {
        // Check if any hit animation is active
        bool hit1 = animator.GetBool("hit1");
        bool hit2 = animator.GetBool("hit2");
        bool hit3 = animator.GetBool("hit3");
        bool RoarSkill = animator.GetBool("RoarSkill");

        if (hit1 || hit2 || hit3 || RoarSkill || isDodging)
        {
            return; // Exit if any animation that interrupts movement is playing
        }

        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.Normalize();

        // Smoothly update the animator movement parameter
        float currentSpeed = moveDirection.magnitude * moveSpeed;
        animator.SetFloat("movement", currentSpeed, 0.1f, Time.deltaTime); // Smooth transition

        if (grounded)
        {
            rb.drag = 5f; // Higher drag for better stopping and control on ground
            rb.AddForce(moveDirection * moveSpeed, ForceMode.Acceleration);
        }
        else
        {
            rb.drag = 0f; // No drag in the air
            rb.AddForce(moveDirection * moveSpeed * airMultiplier, ForceMode.Acceleration);
        }

        // Handle stopping animation
        if (verticalInput == 0 && horizontalInput == 0 && currentSpeed < 0.1f)
        {
            animator.SetTrigger("isStop");
        }

        // Apply gravity
        if (!grounded)
        {
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }
    }
    #endregion

    #region skill player movement
    private void MoveForwardWhileAtk()
    {
        if (animator.GetBool("RoarSkill"))
        {
            return;
        }
        // Mengecek apakah sedang melakukan animasi hit1, hit2, atau hit3
        bool hit1 = animator.GetBool("hit1");
        bool hit2 = animator.GetBool("hit2");
        bool hit3 = animator.GetBool("hit3");
        bool hit4 = animator.GetBool("hit4");

        // Mengatur kecepatan berdasarkan animasi yang sedang aktif
        float forwardSpeed = 0f;
        if (hit1)
        {
            forwardSpeed = .5f;
        }
        else if (hit2)
        {
            forwardSpeed = .5f;
        }
        else if (hit3)
        {
            forwardSpeed = 2f;
        }
        else if (hit4)
        {
            forwardSpeed = 1f;
        }

        // Hanya melakukan pergerakan maju jika karakter berada di tanah dan tidak ada input dari pengguna
        if (grounded)
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
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyMeleeCollider"))
        {
            knockShield -= 5f;
        }
        else if (other.CompareTag("EnemyRangedCollider"))
        {
            knockShield -= 5f;
        }

        if (other.CompareTag("EnemyMeleeCollider") && knockShield <= 20f)
        {
            playerKnockBack();
        }
    }

    void playerKnockBack()
    {
        animator.SetTrigger("getHit");

        moveDirection = orientationForAtk.forward * -1f;
        rb.AddForce(moveDirection.normalized * 10f, ForceMode.Impulse);
        knockShield = 100f;
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
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
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
