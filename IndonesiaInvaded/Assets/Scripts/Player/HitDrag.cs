using UnityEngine;

public class HitDrag : MonoBehaviour
{
    public Transform player; // Referensi ke objek player
    public Transform enemy; // Referensi ke objek enemy
    private Animator animator; // Referensi ke animator controller player

    public float movementSpeed = 5f; // Kecepatan gerak player

    private bool isMoving = false; // Status gerakan player

    void Start()
    {
        // Mengambil komponen Animator dari objek player
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator.GetBool("hit1")) // Memeriksa apakah parameter hit1 true
        {
            MoveToEnemy(); // Memanggil fungsi untuk bergerak ke enemy
        }
        else
        {
            StopMoving(); // Memanggil fungsi untuk menghentikan gerakan jika hit1 false
        }
    }

    void MoveToEnemy()
    {
        if (!isMoving) // Memastikan player tidak sedang dalam proses gerakan
        {
            isMoving = true; // Menandai player sedang dalam proses gerakan

            Vector3 direction = (enemy.position - player.position).normalized; // Menghitung arah menuju enemy
            Vector3 targetPosition = enemy.position - direction * 1.5f; // Menentukan posisi target player

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
        while (Vector3.Distance(player.position, targetPosition) > 0.1f) // Selama player belum mencapai posisi target
        {
            // Menggerakkan player ke arah target
            player.position = Vector3.MoveTowards(player.position, targetPosition, movementSpeed * Time.deltaTime);

            yield return null;
        }

        isMoving = false; // Menandai player telah mencapai posisi target dan tidak lagi dalam proses gerakan
    }
}
