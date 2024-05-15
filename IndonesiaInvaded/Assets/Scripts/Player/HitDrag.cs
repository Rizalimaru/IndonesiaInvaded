using System.Collections;
using UnityEngine;

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
    public float stopDistance = 2f; // Jarak untuk berhenti mendekati musuh

    private bool isMoving = false; // Status gerakan player
    public Transform nearestEnemy; // Referensi ke musuh terdekat

    public Transform combatLookAt; // Referensi ke titik yang harus dilihat oleh player saat berada dalam mode combat

    void Start()
    {
        // Mengambil komponen Animator dari objek player
        animator = player.GetComponent<Animator>();
    }

    void Update()
    {
        // Mendeteksi musuh terdekat
        DetectNearestEnemy();

        // Memeriksa apakah parameter hit1 true dan musuh berada dalam jarak deteksi
        if (animator.GetBool("hit1") && nearestEnemy != null && Vector3.Distance(player.position, nearestEnemy.position) <= detectionRadius)
        {
            if (Vector3.Distance(player.position, nearestEnemy.position) > stopDistance)
            {
                MoveToEnemy(); // Memanggil fungsi untuk bergerak ke enemy
            }
            else
            {
                StopMoving(); // Memanggil fungsi untuk menghentikan gerakan jika sudah dalam jarak 2 unit dari musuh
            }
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
            StartCoroutine(MoveToEnemyCoroutine()); // Memulai proses gerakan player ke musuh
        }
    }

    private IEnumerator MoveToEnemyCoroutine()
    {
        LookAtEnemy(); // Menghadap ke arah musuh

        Vector3 startPosition = player.position; // Simpan posisi awal player
        Vector3 moveDirection = (nearestEnemy.position - startPosition).normalized; // Hitung arah pergerakan ke musuh
        Vector3 targetPosition = nearestEnemy.position - moveDirection * 1.5f; // Hitung posisi target berdasarkan jarak yang ditentukan

        targetPosition.y = player.position.y; // Memastikan target posisi tetap pada level y yang sama
        float distanceToMove = Vector3.Distance(startPosition, targetPosition); // Hitung jarak yang harus ditempuh
        float duration = distanceToMove / movementSpeed; // Hitung durasi pergerakan berdasarkan jarak dan kecepatan

        float timeElapsed = 0f;
        while (timeElapsed < duration && Vector3.Distance(player.position, nearestEnemy.position) > stopDistance) // Tambahkan kondisi jarak
        {
            // Interpolasi pergerakan
            player.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        player.position = targetPosition; // Pastikan posisi player benar-benar mencapai posisi target
        isMoving = false; // Menandai player tidak lagi dalam proses gerakan
    }

    public void LookAtEnemy()
    {
        if (nearestEnemy != null)
        {
            Vector3 targetDirection = nearestEnemy.position - player.position;
            targetDirection.y = 0f; // Keep the rotation in the horizontal plane
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            player.rotation = Quaternion.Slerp(player.rotation, rotation, movementSpeed * Time.deltaTime);
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
}
