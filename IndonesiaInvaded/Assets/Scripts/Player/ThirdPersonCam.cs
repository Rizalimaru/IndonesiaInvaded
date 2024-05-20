using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode LockCamera = KeyCode.LeftAlt;
    private KeyCode RangedAttackKey = KeyCode.Mouse1;

    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    private SkillManager skillManager;
    Transform nearestEnemy;
    CinemachineVirtualCamera vcam;

    [Header("Rotation To Enemy")]
    private Animator animator;
    public Transform target;
    public float rotationToEnemySpeed;
    public HitDrag hitDrag;

    [Header("Settings")]
    public float rotationSpeed;
    public float RangedAtkDetectionRadius = 50f;
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
        vcam = GetComponent<CinemachineVirtualCamera>();
        animator = player.GetComponent<Animator>();
        skillManager = SkillManager.instance; // Initialize SkillManager instance
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        bool RangeAtkAktif = Input.GetKey(RangedAttackKey); // Use GetKey to check if the key is being held down
        // Toggle cursor lock mode
        if (Input.GetKeyDown(LockCamera))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = Cursor.lockState == CursorLockMode.None;
        }
        // Switch styles
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraStyle(CameraStyle.Topdown);

        // Rotate orientation
        if (RangeAtkAktif)
        {
            DetectNearestEnemyForSkill(); // Update the nearest enemy
            if (nearestEnemy != null)
            {
                Vector3 viewDir = nearestEnemy.position - new Vector3(transform.position.x, nearestEnemy.position.y, transform.position.z);
                orientation.forward = viewDir.normalized;
                LookAtNearestEnemy(); // Ensure player looks at nearest enemy
            }
        }
        else
        {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDir.normalized;
        }

        // Rotate player object
        if (currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Disable horizontal and vertical input if RoarSkill is active
            if (animator.GetBool("RoarSkill"))
            {
                horizontalInput = 0f;
                verticalInput = 0f;
            }

            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (RangeAtkAktif && verticalInput < 0)
            {
                // Ignore negative vertical input while RangeAtkAktif
                inputDir = orientation.right * horizontalInput;
            }

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        else if (currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }

        // Check if hit1 parameter is true and the current style is Basic
        if (currentStyle == CameraStyle.Basic && animator.GetBool("hit1") && hitDrag.nearestEnemy != null)
        {
            // Rotate player towards the target
            LookAtEnemyForHitdrag();
        }
        else if (animator.GetBool("RoarSkill"))
        {
            LookAtEnemy();
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

    #region CameraLook Function

    public void LookAtEnemyForHitdrag()
    {
        Vector3 targetDirection = hitDrag.nearestEnemy.position - player.position;
        targetDirection.y = 0f; // Keep the rotation in the horizontal plane
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        playerObj.rotation = Quaternion.Slerp(playerObj.rotation, rotation, rotationToEnemySpeed * Time.deltaTime);
    }

    void LookAtEnemy()
    {
        Vector3 targetDirection = skillManager.nearestEnemy.position - player.position;
        targetDirection.y = 0f; // Keep the rotation in the horizontal plane
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        playerObj.rotation = Quaternion.Slerp(playerObj.rotation, rotation, rotationToEnemySpeed * Time.deltaTime);
    }

    public void LookAtNearestEnemy()
    {
        if (nearestEnemy != null)
        {
            Vector3 targetDirection = nearestEnemy.position - player.position;
            targetDirection.y = 0f; // Keep the rotation in the horizontal plane
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            playerObj.rotation = Quaternion.Slerp(playerObj.rotation, rotation, rotationToEnemySpeed * Time.deltaTime);
        }
    }

    public IEnumerator LookAtEnemyForSkill()
    {
        if (hitDrag.nearestEnemy != null)
        {
            yield return new WaitForSeconds(1f);
            Vector3 targetDirection = skillManager.nearestEnemy.position - player.position;
            targetDirection.y = 0f; // Keep the rotation in the horizontal plane
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            playerObj.rotation = Quaternion.Slerp(playerObj.rotation, rotation, rotationToEnemySpeed * Time.deltaTime);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

    #region enemy detection region
    public void DetectNearestEnemyForSkill()
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, RangedAtkDetectionRadius); // Detect all colliders within the radius

        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(player.position, collider.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearest = collider.transform;
                }
            }
        }

        nearestEnemy = nearest; // Set the nearest enemy as reference
    }
    #endregion
}
