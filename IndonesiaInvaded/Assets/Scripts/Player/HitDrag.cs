using UnityEngine;

public class HitDrag : MonoBehaviour
{
    public Transform player; // Referensi ke objek player
    private Animator animator; // Referensi ke animator controller player

    public float movementSpeed = 5f; // Kecepatan gerak player
    public float detectionRadius = 5f; // Jarak deteksi musuh terdekat

    private bool isMoving = false; // Status gerakan player
    private Transform nearestEnemy; // Referensi ke musuh terdekat

    void Start()
    {
        // Mengambil komponen Animator dari objek player
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Mendeteksi musuh terdekat
        DetectNearestEnemy();

        // Memeriksa apakah parameter hit1 true dan musuh berada dalam jarak deteksi
        if (animator.GetBool("hit1") && nearestEnemy != null && Vector3.Distance(new Vector3(player.position.x, 0, player.position.z), new Vector3(nearestEnemy.position.x, 0, nearestEnemy.position.z)) <= detectionRadius)
        {
            MoveToEnemy(); // Memanggil fungsi untuk bergerak ke enemy
        }
        else
        {
            StopMoving(); // Memanggil fungsi untuk menghentikan gerakan jika hit1 false atau musuh di luar jarak deteksi
        }
    }

    void DetectNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, detectionRadius); // Mendeteksi semua collider dalam radius

        float shortestDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(new Vector3(player.position.x, 0, player.position.z), new Vector3(collider.transform.position.x, 0, collider.transform.position.z));
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearest = collider.transform;
                }
            }
        }

        nearestEnemy = nearest; // Menetapkan musuh terdekat sebagai referensi
    }

    void MoveToEnemy()
    {
        if (!isMoving) // Memastikan player tidak sedang dalam proses gerakan
        {
            isMoving = true; // Menandai player sedang dalam proses gerakan

            Vector3 direction = (nearestEnemy.position - player.position).normalized; // Menghitung arah menuju enemy
            Vector3 targetPosition = nearestEnemy.position - new Vector3(direction.x, 0, direction.z) * 1.5f; // Menentukan posisi target player

            StartCoroutine(MovePlayer(targetPosition)); // Memulai proses gerakan player ke posisi target
        }
    }

    void StopMoving()
    {
        if (isMoving) // Memastikan player sedang dalam proses gerakan
        {
            StopCoroutine(MovePlayer(Vector3.zero)); // Menghentikan proses gerakan player
            isMoving = false; // Menandai player tidak lagi dalam proses gerakan
        }
    }

    System.Collections.IEnumerator MovePlayer(Vector3 targetPosition)
    {
        while (Vector3.Distance(new Vector3(player.position.x, 0, player.position.z), new Vector3(targetPosition.x, 0, targetPosition.z)) > 0.1f) // Selama player belum mencapai posisi target
        {
            // Menggerakkan player ke arah target
            player.position = Vector3.MoveTowards(new Vector3(player.position.x, 0, player.position.z), new Vector3(targetPosition.x, 0, targetPosition.z), movementSpeed * Time.deltaTime);

            yield return null;
        }

        isMoving = false; // Menandai player telah mencapai posisi target dan tidak lagi dalam proses gerakan
    }
}
