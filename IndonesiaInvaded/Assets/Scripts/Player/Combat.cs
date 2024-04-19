using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{   
    private Animator anim;
    private Rigidbody rb;
    public float cooldownTime = 2f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    public LayerMask enemyLayer; // Layer yang berisi musuh
    public float attackForce = 10f; // Gaya dorongan saat menyerang
    public float attackRange = 2f; // Jarak serangan

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {   
        // Setelah animasi "hit1" selesai, kembalikan ke keadaan awal
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            anim.SetBool("hit1", false);
        }
        // Setelah animasi "hit2" selesai, kembalikan ke keadaan awal
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            anim.SetBool("hit2", false);
        }
        // Setelah animasi "hit3" selesai, kembalikan ke keadaan awal
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            anim.SetBool("hit3", false);
            noOfClicks = 0;
        }

        // Jika sudah melewati batas waktu maksimum combo, kembalikan jumlah klik ke 0
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        // Cooldown time
        if (Time.time > nextFireTime)
        {
            // Check for mouse input
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
    }

    void OnClick()
    {
        // Pengecekan musuh terdekat
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        if (nearbyEnemies.Length > 0)
        {
            // Ambil musuh terdekat dan berikan gaya dorongan ke arahnya
            Transform nearestEnemy = GetNearestEnemy(nearbyEnemies);
            if (nearestEnemy != null)
            {
                Vector3 direction = nearestEnemy.position - transform.position;
                direction.y = 0; // Tetapkan 0 agar hanya bergerak di bidang horizontal
                direction.Normalize();
                rb.AddForce(direction * attackForce, ForceMode.Impulse);
            }
        }

        // Memulai serangan berdasarkan jumlah klik
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            anim.SetBool("hit1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            anim.SetBool("hit1", false);
            anim.SetBool("hit2", true);
        }
        if (noOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && anim.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            anim.SetBool("hit2", false);
            anim.SetBool("hit3", true);
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
