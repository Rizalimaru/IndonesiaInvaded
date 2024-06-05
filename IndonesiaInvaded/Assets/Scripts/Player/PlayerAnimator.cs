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
            Destroy(gameObject); // Un-comment this line if you want to enforce singleton pattern strictly
        }
    }

    private void FixedUpdate()
    {
        UpdateGroundedStatus();
        UpdateAnimator();
    }

    private void UpdateGroundedStatus()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.8f, whatIsGround | whatIsGround2);
    }

    private void UpdateAnimator()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        bool verticalUp = Input.GetKey(KeyCode.W);
        bool verticalDown = Input.GetKey(KeyCode.S);
        bool horizontalLeft = Input.GetKey(KeyCode.A);
        bool horizontalRight = Input.GetKey(KeyCode.D);

        bool modeRange = Input.GetKey(rangedAttackKey);

        if (!modeRange)
        {
            HandleMovement(horizontalInput, verticalInput, verticalUp, verticalDown, horizontalLeft, horizontalRight);
        }
        else
        {
            HandleRangedMovement(verticalUp, verticalDown, horizontalLeft, horizontalRight);
        }

        anim.SetFloat("movementZ", currentMovementZ);
        anim.SetFloat("movementX", currentMovementX);
        anim.SetBool("isJump", !grounded);
        anim.SetBool("isSprint", IsSprinting());
        anim.SetBool("isRun", currentMovementZ == maxMovement);
        anim.SetBool("isIdle", currentMovementZ == 0);
        anim.SetBool("isDodge", playerMovement.IsDodging);

        if(playerMovement.PlungeGaSih == true)
        {
            StartCoroutine(PlungeAnimation());
        }
    }

    IEnumerator PlungeAnimation()
    {   
        anim.SetTrigger("Spinning");
        yield return new WaitForSeconds(.5f);
        anim.SetTrigger("PlungeAtk");
    }

    private void HandleMovement(float horizontalInput, float verticalInput, bool verticalUp, bool verticalDown, bool horizontalLeft, bool horizontalRight)
    {
        anim.SetBool("RangeAtkAktif", false);

        float targetMovement = Mathf.Clamp01(Mathf.Sqrt(horizontalInput * horizontalInput + verticalInput * verticalInput));

        if (IsSprinting())
        {
            currentMovementZ = Mathf.MoveTowards(currentMovementZ, maxMovement, acceleration * Time.deltaTime);
        }
        else
        {
            currentMovementZ = Mathf.MoveTowards(currentMovementZ, targetMovement, acceleration * Time.deltaTime);
        }

        if (!verticalUp && !verticalDown && !horizontalLeft && !horizontalRight)
        {
            currentMovementZ = Mathf.MoveTowards(currentMovementZ, 0, deceleration * Time.deltaTime);
            currentMovementX = Mathf.MoveTowards(currentMovementX, 0, deceleration * Time.deltaTime);
        }
    }

    private void HandleRangedMovement(bool verticalUp, bool verticalDown, bool horizontalLeft, bool horizontalRight)
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

    private bool IsSprinting()
    {
        return Input.GetKey(KeyCode.LeftShift) && currentMovementZ > 0;
    }

}
