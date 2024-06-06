using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private Combat combat;
    public static PlayerMovement instance;
    private SkillManager skillManager;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float rangedAtkMoveSpeed = 5f;
    public float sprintSpeed;
    public float groundDrag;
    private bool isStopping = false;
    public bool canMove = true;
    bool bisaPlungeAtk;
    bool SedangRange;
    bool sedangKnock = false;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public GameObject GroundPoundVFX;
    bool readyToJump;
    public GameObject SkillRoarCollider;

    [Header("Dodge")]
    [SerializeField] AnimationCurve dodgeCurve;
    bool isDodging;
    bool canDodge;
    public float dodgeDistance = 5f; // Jarak dodge
    public float dodgeSpeed = 10f;   // Kecepatan dodge
    public float dodgeCooldown = 1f; // Waktu cooldown setelah dodge
    public float dodgeTimer;
    private Vector3 dodgeDirection;
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
    public float knockBackForce = 20f;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Un-comment this line if you want to enforce singleton pattern strictly
        }
    }
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

#region  updateRegion
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

        if (sedangKnock)
        {
            knockShield = 100f;
        }
    }
#endregion

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        bool SedangRange = Input.GetKey(rangedAtkKey);

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded && combat.isAttacking == false && !SedangRange)
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
        if (Input.GetKeyDown(dodgeKey) && !isDodging && grounded)
        {   
            StartCoroutine(Dodge());
        }
    }
#region getBoolRegion
        public bool IsDodging
        {
            get { return isDodging; }
        }
        public bool CanDodge
        {
            get { return canDodge; }
        }
        public float magnitude
        {
            get { return moveDirection.magnitude; }
        }
        public bool PlungeGaSih
        {
            get { return bisaPlungeAtk; }
        }
        public bool SedangRangeAtk
        {
            get { return SedangRange; }
        }
        public bool lagiKnock
        {
            get { return sedangKnock; }
        }
#endregion
    private IEnumerator Dodge()
    {
        AudioManager._instance.PlaySFX("StatePlayer", 1);
        isDodging = true;
        canDodge = false;

        // Menyimpan arah dodge berdasarkan input pemain pada awal dodge
        Vector3 initialDodgeDirection = orientationForAtk.forward;

        if (moveDirection.magnitude == 0) // Jika tidak ada input, dodge ke belakang
        {
            initialDodgeDirection = -orientationForAtk.forward;
        }

        if (moveDirection.magnitude == 0)
        {
            animator.SetTrigger("Backflip");
        }
        else
        {
            animator.SetTrigger("Dodge");
        }

        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        // Variabel untuk menentukan durasi dodge berdasarkan kecepatan dan jarak dodge
        float dodgeDuration = dodgeDistance / dodgeSpeed;

        while (Time.time < startTime + dodgeDuration)
        {
            // Update arah dodge setiap frame
            Vector3 currentDodgeDirection = moveDirection.magnitude == 0 ? -orientationForAtk.forward : orientationForAtk.forward;

            // Lakukan raycast untuk memeriksa apakah ada objek di depan karakter
            RaycastHit hit;
            if (Physics.Raycast(startPosition, currentDodgeDirection, out hit, dodgeDistance, LayerMask.GetMask("Property"))) // Ubah layerMask sesuai dengan layer properti yang ingin dihindari
            {
                // Jika ada objek di depan, hentikan dodge
                isDodging = false;
                yield break;
            }

            Vector3 targetPosition = startPosition + currentDodgeDirection * dodgeDistance;

            rb.MovePosition(Vector3.Lerp(startPosition, targetPosition, (Time.time - startTime) / dodgeDuration));
            yield return null;
        }

        isDodging = false;
        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }

#region  stateHandleRegion
    private void StateHandler()
    {
        bool SedangRange = Input.GetKey(rangedAtkKey);
        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey) && !SedangRange)
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
#endregion

    #region basic player movement
    private void MovePlayer()
    {   
        if (!canMove) return;
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
            bisaPlungeAtk = false;
            rb.drag = 5f; // Higher drag for better stopping and control on ground
            rb.AddForce(moveDirection * moveSpeed, ForceMode.Acceleration);
        }
        else
        {
            rb.drag = 0f; // No drag in the air
            rb.AddForce(moveDirection * airMultiplier, ForceMode.Acceleration);
        }

        // Apply gravity
        if (!grounded)
        {
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }

        if(!grounded && Input.GetMouseButton(0) && bisaPlungeAtk == false)
        {   
            StartCoroutine(plungeAtk());
            bisaPlungeAtk = true;
        }

    }

    IEnumerator plungeAtk()
    {
        AudioManager._instance.PlaySFX("PlungeAttack", 0);
        rb.AddForce(Vector3.up * 7f, ForceMode.Impulse);
        yield return new WaitForSeconds(.5f);
        rb.AddForce(Vector3.down * (gravity * 70), ForceMode.Acceleration);
        yield return new WaitForSeconds(.5f);
        SpawnGoundPundVFX();
        AudioManager._instance.PlaySFX("PlungeAttack", 1);
        SpawnRoarCollider();
    }

    public void SpawnGoundPundVFX()
    {

        GameObject groundPound = Instantiate(GroundPoundVFX, transform.position, transform.rotation) as GameObject;
        Destroy(groundPound, 2f);
    }

    void SpawnRoarCollider()
    {
        
        GameObject roarCollider = Instantiate(SkillRoarCollider, transform.position, transform.rotation) as GameObject;
        Destroy(roarCollider, 3f);
        
    }
    #endregion

    #region skill player movement
    private void MoveForwardWhileAtk()
    {   
        if (!canMove) return;
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

#region  OnCollosionAndTriggerRegion
    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("EnemyMeleeCollider"))
        // {
        //     knockShield -= 5f;
        // }
        // else if (other.CompareTag("EnemyRangedCollider"))
        // {
        //     knockShield -= 5f;
        // }

        // if (other.CompareTag("EnemyMeleeCollider") && knockShield <= 20f)
        // {
        //     playerKnockBack();
        // }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;
        if (other.CompareTag("BossMeleeCollider"))
        {
            knockShield -= 10f;
        }
        if (other.CompareTag("BossMeleeCollider") && sedangKnock == false && !isDodging && knockShield <= 20f)
        {   
            sedangKnock = true;
            CameraShaker.instance.CameraShake(5f, 0.6f);
            ThirdPersonCam.instance.GetBisaRotasi = false;
            // Menentukan arah knockback berdasarkan posisi tabrakan
            Vector3 direction = (transform.position - collision.transform.position).normalized;
            // Menerapkan kekuatan knockback pada Rigidbody
            rb.AddForce(direction * knockBackForce, ForceMode.Impulse);
            animator.SetBool("knocked", true);
            StartCoroutine(ImuneTime());
        }
    }
#endregion
    IEnumerator ImuneTime()
    {
        canMove = false;
        yield return new WaitForSeconds(4);
        canMove = true;
        animator.SetBool("knocked", false);
        ThirdPersonCam.instance.GetBisaRotasi = true;
        sedangKnock = false;
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
