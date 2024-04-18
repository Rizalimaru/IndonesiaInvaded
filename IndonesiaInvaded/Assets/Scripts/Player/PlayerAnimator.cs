using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator anim;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
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

        bool isRun = Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && isRun; // Check if player is sprinting

        anim.SetBool("isRun", isRun && !isSprinting); // Set isRun parameter
        anim.SetBool("isSprint",isSprinting); // Set isSprint parameter
        anim.SetBool("isIdle", !isRun && grounded && !isSprinting); // Set isIdle parameter

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
