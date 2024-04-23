using UnityEngine;
using Cinemachine;

public class LockEnemy : MonoBehaviour
{
    Animator animator;
    public CinemachineVirtualCamera playerCamera; // Kamera pemain
    public GameObject lockOnCamera; // GameObject yang berisi Cinemachine Virtual Camera untuk mode kunci musuh
    public LayerMask enemyLayer; // Layer yang berisi musuh
    public float lockOnRange = 10f; // Jarak maksimum untuk mengunci musuh
    public float minLockOnRange = 5f; // Jarak minimum agar bisa masuk ke mode kunci musuh
    public KeyCode lockOnKey = KeyCode.Mouse2; // Tombol untuk mengaktifkan mode kunci musuh

    private Transform currentTarget; // Musuh yang sedang terkunci
    private bool isLockedOn = false; // Status mode kunci musuh

    void Start()
    {
        // Mulai dengan menonaktifkan GameObject yang berisi kamera mode kunci musuh
        lockOnCamera.SetActive(false);
    }

    void Update()
    {
        // Cek input untuk mengaktifkan mode kunci musuh
        if (Input.GetKeyDown(lockOnKey))
        {
            if (!isLockedOn)
            {
                // Cek apakah ada musuh dalam jarak minimum
                Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, minLockOnRange, enemyLayer);
                if (nearbyEnemies.Length > 0)
                {
                    isLockedOn = true;
                    // Aktifkan GameObject yang berisi kamera mode kunci musuh jika belum aktif
                    lockOnCamera.SetActive(true);
                    // Nonaktifkan kamera pemain
                    playerCamera.enabled = false;
                }
            }
            else
            {
                isLockedOn = false;
                // Nonaktifkan GameObject yang berisi kamera mode kunci musuh jika tidak lagi dalam mode kunci musuh
                lockOnCamera.SetActive(false);
                // Aktifkan kamera pemain kembali
                playerCamera.enabled = true;
                currentTarget = null; // Reset target saat mode kunci musuh dinonaktifkan
            }
        }

        // Jika sedang dalam mode kunci musuh
        if (isLockedOn)
        {
            // Cek apakah ada musuh di sekitar
            Collider[] enemies = Physics.OverlapSphere(transform.position, lockOnRange, enemyLayer);
            if (enemies.Length > 0)
            {
                // Ambil musuh terdekat sebagai target
                Transform nearestEnemy = GetNearestEnemy(enemies);
                if (nearestEnemy != null)
                {
                    currentTarget = nearestEnemy;
                }
            }
            else
            {
                // Jika tidak ada musuh dalam jarak, keluar dari mode kunci musuh
                isLockedOn = false;
                lockOnCamera.SetActive(false);
                playerCamera.enabled = true;
                currentTarget = null;
                return; // Keluar dari fungsi Update
            }

            // Jika target ada, atur kamera mengunci ke target
            if (currentTarget != null)
            {
                // Mengikuti target musuh dengan kamera Cinemachine
                lockOnCamera.GetComponent<CinemachineVirtualCamera>().LookAt = currentTarget;
            }
        }
    }

    // Fungsi untuk mendapatkan musuh terdekat dari array collider musuh
    Transform GetNearestEnemy(Collider[] enemies)
    {
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }
}
