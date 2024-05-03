using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using IndonesiaInvaded;

public class SkillManager : MonoBehaviour
{
    // Singleton instance
    private ThirdPersonCam thirdPersonCam;
    private HitDrag hitDrag;
    private PlayerMovement playerMovement;
    private CameraZoom cameraZoom;
    private Animator animator;
    public static SkillManager instance;
    public Transform playerObj;

    [Header("Skill 1")]
    public Image skillImage1;
    public float cooldown1 = 5;
    private bool isCooldown1 = false;
    public KeyCode skill1Key;
    public CameraShake cameraShake;
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    public float SkillDetectionRadius = 10f;
    public float movementSpeed = 5f;
    private float distanceToMove;
    public float rotationToEnemySpeed = 5.0f;

    [Header("Skill 2")]
    public Image skillImage2;
    public float cooldown2 = 8; // Cooldown for Skill 2
    private bool isCooldown2 = false;
    public KeyCode skill2Key;

    [Header("Slow Motion Effect")]
    private bool isSlowMotionActive = false;
    public float slowMotionDuration = 1f; // Durasi slow motion dalam detik
    public float slowMotionTimeScale = 0.5f; // Skala waktu selama slow motion

    [Header("Skill Detection")]
    public Transform player;
    private Transform nearestEnemy;

    private void Awake()
    {   

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {   
        animator = GetComponent<Animator>();
        skillImage1.fillAmount = 0;
        skillImage2.fillAmount = 0;
    }

    private void Update()
    {
        Skill1();
        Skill2(); // Call Skill2 method in the Update loop
        distanceToMove = Mathf.Sqrt(Mathf.Pow((player.position.x - nearestEnemy.position.x),2) + Mathf.Pow((player.position.z - nearestEnemy.position.z),2) );
    }

    public void UseSkill1()
    {
        PlayerAttribut player = PlayerAttribut.instance;
        if (player != null && !isCooldown1 && player.currentSP >= 30)
        {   
            animator.SetBool("RoarSkill", true);
            StartCoroutine(DelayToCharge(1.5f));
            player.currentSP -= 30;
            StartCoroutine(StartSlowMotion());
            Debug.Log("Skill 1 activated!");
            player.skillBar.SetSkill(player.currentSP);
            // Start cooldown
            StartCoroutine(CooldownSkill1());
            // Zoom camera in
            //StartCoroutine(DelayZoomSkill1());
        }
        else if (player.currentSP < 30)
        {
            Debug.Log("Not enough SP for Skill 1!");
        }
    }

    public void UseSkill2()
    {
        PlayerAttribut player = PlayerAttribut.instance;
        if (player != null && !isCooldown2 && player.currentSP >= 50) // Check if enough SP and skill is not on cooldown
        {
            player.currentSP -= 50;
            Debug.Log("Skill 2 activated!");
            player.skillBar.SetSkill(player.currentSP);
            // Start cooldown
            StartCoroutine(CooldownSkill2());
        }
        else if (player.currentSP < 50)
        {
            Debug.Log("Not enough SP for Skill 2!");
        }
    }

    private IEnumerator DelayZoomSkill1()
    {
        CameraZoom cameraZoom = FindObjectOfType<CameraZoom>(); // Mendapatkan instance dari CameraZoom
        if(cameraZoom != null)
        {
            cameraZoom.ZoomIn(1.5f); // Mengatur durasi zoom in ke 1.5 detik
        }

        yield return new WaitForSeconds(5.0f); // Menunggu selama 2 detik sebelum melakukan zoom out

        if(cameraZoom != null)
        {
            cameraZoom.ZoomOut(1.5f); // Mengatur durasi zoom out ke 1.5 detik
        }
    }

    private IEnumerator CooldownSkill1()
    {
        isCooldown1 = true;
        float cooldownTimer = cooldown1;
        while (cooldownTimer > 0)
        {
            skillImage1.fillAmount = cooldownTimer / cooldown1;
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }
        isCooldown1 = false;
        skillImage1.fillAmount = 0;
    }

    private IEnumerator DelayToCharge(float delay)
    {
        yield return new WaitForSeconds(delay);

        animator.SetTrigger("ChargeAtk");
        animator.SetBool("RoarSkill", false);

        DetectNearestEnemyForSkill(); // Deteksi musuh terdekat
        StartCoroutine(MoveToEnemyAfterCharge()); // Mulai pergerakan ke musuh setelah ChargeAtk
    }

    private IEnumerator MoveToEnemyAfterCharge()
    {
        if (nearestEnemy != null) // Memastikan ada musuh terdekat
        {   
            yield return new WaitForSeconds(.5f); // Menunggu 1 detik sebelum pergerakan dimulai
            LookAtEnemy(); // Menghadap ke arah musuh
            Vector3 startPosition = player.position; // Simpan posisi awal player
            Vector3 moveDirection = (nearestEnemy.position - startPosition).normalized; // Hitung arah pergerakan ke musuh

             // Jarak yang ingin dijelajahi (misalnya 5 meter)
            Vector3 targetPosition = startPosition + moveDirection * distanceToMove; // Hitung posisi target berdasarkan jarak yang ditentukan

            float duration = distanceToMove / movementSpeed; // Hitung durasi pergerakan berdasarkan jarak dan kecepatan

            float timeElapsed = 0f;
            while (timeElapsed < duration) // Pergerakan berdasarkan durasi
            {
                // Interpolasi pergerakan
                player.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            // Pastikan posisi player benar-benar mencapai posisi target
            player.position = targetPosition;
        }
    }

    private IEnumerator CooldownSkill2()
    {
        isCooldown2 = true;
        float cooldownTimer = cooldown2;
        while (cooldownTimer > 0)
        {
            skillImage2.fillAmount = cooldownTimer / cooldown2;
            cooldownTimer -= Time.deltaTime;
            yield return null;
        }
        isCooldown2 = false;
        skillImage2.fillAmount = 0;
    }

    private void Skill1()
    {
        if (Input.GetKeyDown(skill1Key) && !isCooldown1)
        {
            UseSkill1();
        }
    }

    private void Skill2()
    {
        if (Input.GetKeyDown(skill2Key) && !isCooldown2) // Check for key press and skill cooldown
        {
            UseSkill2();
        }
    }

    public void DetectNearestEnemyForSkill()
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, SkillDetectionRadius); // Mendeteksi semua collider dalam radius

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

        nearestEnemy = nearest; // Menetapkan musuh terdekat sebagai referensi
    }

    public void LookAtEnemy()
    {
        Vector3 targetDirection = nearestEnemy.position - player.position;
        targetDirection.y = 0f; // Keep the rotation in the horizontal plane
        Quaternion rotation = Quaternion.LookRotation(targetDirection);
        playerObj.rotation = Quaternion.Slerp(playerObj.rotation, rotation, rotationToEnemySpeed * Time.deltaTime);
    }

    public IEnumerator StartSlowMotion()
    {
        if (!isSlowMotionActive)
        {   
            yield return new WaitForSeconds(1f);
            // Mengaktifkan slow motion
            Time.timeScale = slowMotionTimeScale;
            isSlowMotionActive = true;

            // Membatalkan slow motion setelah beberapa detik
            Invoke("StopSlowMotion", slowMotionDuration);
        }
    }

    public void StopSlowMotion()
    {
        // Menonaktifkan slow motion
        Time.timeScale = 1f;
        isSlowMotionActive = false;
    }
}
