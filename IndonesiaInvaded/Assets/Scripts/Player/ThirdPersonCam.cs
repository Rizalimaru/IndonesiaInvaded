using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{   
    [Header("Keybinds")]
    public KeyCode LockCamera = KeyCode.LeftAlt;

    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    private SkillManager skillManager;


    [Header("Rotation To Enemy")]
    private Animator animator;
    public Transform target;
    public float rotationToEnemySpeed;
    public HitDrag hitDrag;

    [Header("Settings")]
    public float rotationSpeed;
    public Transform combatLookAt;
    public GameObject thirdPersonCam;
    public GameObject combatCam;
    public GameObject topDownCam;

    public CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }

    private void Start()
    {   
        animator = player.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {   
        // Toggle cursor lock mode
        if (Input.GetKeyDown(LockCamera))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = Cursor.lockState == CursorLockMode.None;
        }

        // switch styles
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraStyle(CameraStyle.Topdown);

        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // rotate player object
        if(currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }

        // Check if hit1 parameter is true and the current style is Basic
        if (currentStyle == CameraStyle.Basic && animator.GetBool("hit1") && hitDrag.nearestEnemy !=null )
        {
            // Rotate player towards the target
            LookAtEnemyForHitdrag();
        }else if (animator.GetBool("RoarSkill"))
        {
            StartCoroutine(LookAtEnemyForSkill());
        }
    }


    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);
        if (newStyle == CameraStyle.Topdown) topDownCam.SetActive(true);

        currentStyle = newStyle;
    }

    public void LookAtEnemyForHitdrag()
    {
        Vector3 targetDirection = hitDrag.nearestEnemy.position - player.position;
        targetDirection.y = 0f; // Keep the rotation in the horizontal plane
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        playerObj.rotation = Quaternion.Slerp(playerObj.rotation, rotation, rotationToEnemySpeed * Time.deltaTime);
    }

    public IEnumerator LookAtEnemyForSkill()
    {   
        yield return new WaitForSeconds(1f);
        Vector3 targetDirection = hitDrag.nearestEnemy.position - player.position;
        targetDirection.y = 0f; // Keep the rotation in the horizontal plane
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        playerObj.rotation = Quaternion.Slerp(playerObj.rotation, rotation, rotationToEnemySpeed * Time.deltaTime);
    }

}
