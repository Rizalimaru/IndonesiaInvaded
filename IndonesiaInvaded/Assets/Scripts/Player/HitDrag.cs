using UnityEngine;
using System.Collections;

public class HitDrag : MonoBehaviour
{   
    public enum TagEnemyOptions
    {
        Enemy,
        EnemyMeleeCollider,
    }
    public TagEnemyOptions tagEnemyOptions;
    public Transform player; // Referensi ke objek player
    private Animator animator; // Referensi ke animator controller player

    public float movementSpeed = 5f; // Kecepatan gerak player
    public float detectionRadius = 5f; // Jarak deteksi musuh terdekat

    private bool isMoving = false; // Status gerakan player
    public Transform nearestEnemy; // Referensi ke musuh terdekat

    public Transform combatLookAt; // Referensi ke titik yang harus dilihat oleh player saat berada dalam mode combat
    KeyCode rangedAtkKey = KeyCode.Mouse1;
    private PlayerMovement playerMovement;
    void Start()
    {   
        playerMovement = FindObjectOfType<PlayerMovement>();
        // Mengambil komponen Animator dari objek player
        animator = player.GetComponent<Animator>();
    }

    void Update()
    {   
        bool rangeAtkAktif = Input.GetKey(rangedAtkKey);
        // Mendeteksi musuh terdekat
        DetectNearestEnemy();

        // Memeriksa apakah parameter hit1 true dan musuh berada dalam jarak deteksi
        if (animator.GetBool("hit1") && nearestEnemy != null && Vector3.Distance(player.position, nearestEnemy.position) <= detectionRadius && !rangeAtkAktif && playerMovement.CanDodge)
        {
            MoveToEnemy(); // Memanggil fungsi untuk bergerak ke enemy
        }
        else
        {
            StopMoving(); // Memanggil fungsi untuk menghentikan gerakan jika hit1 false atau musuh di luar jarak deteksi
        }
    }

    public void DetectNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, detectionRadius); // Mendeteksi semua collider dalam radius

        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(tagEnemyOptions.ToString()))
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

    public void MoveToEnemy()
    {
        if (!isMoving && nearestEnemy != null) // Memastikan player tidak sedang dalam proses gerakan dan terdapat musuh terdekat
        {
            isMoving = true; // Menandai player sedang dalam proses gerakan

            Vector3 direction = (nearestEnemy.position - player.position).normalized; // Menghitung arah menuju enemy
            Vector3 targetPosition = nearestEnemy.position - direction * 1.5f; // Menentukan posisi target player

            // Mengatur posisi target dengan nilai y tetap dari posisi player
            targetPosition.y = player.position.y;

            StartCoroutine(MovePlayer(targetPosition)); // Memulai proses gerakan player ke posisi target
        }
    }


    void StopMoving()
    {
        if (isMoving) // Memastikan player sedang dalam proses gerakan
        {
            StopAllCoroutines(); // Menghentikan semua proses gerakan player
            isMoving = false; // Menandai player tidak lagi dalam proses gerakan
        }
    }

    IEnumerator MovePlayer(Vector3 targetPosition)
    {
        while (Vector3.Distance(player.position, targetPosition) > 0.5f) // Selama player belum mencapai posisi target
        {
            // Menggerakkan player ke arah target
            player.position = Vector3.MoveTowards(player.position, targetPosition, movementSpeed * Time.deltaTime);

            yield return null;
        }

        isMoving = false; // Menandai player telah mencapai posisi target dan tidak lagi dalam proses gerakan
    }
}