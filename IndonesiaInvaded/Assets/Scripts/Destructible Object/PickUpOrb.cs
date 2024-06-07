using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpOrb : MonoBehaviour
{
    [SerializeField] private int hpAmount = 200; // Jumlah HP yang akan ditambahkan saat orb diambil
    [SerializeField] private int spAmount = 50; // Jumlah SP yang akan ditambahkan saat orb diambil

    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;

        if (other.CompareTag("Player")) // Pastikan objek yang bersentuhan memiliki tag "Player"
        {
            PlayerAttribut player = other.GetComponent<PlayerAttribut>(); // Dapatkan komponen PlayerAttribut dari pemain yang menangkap orb
            if (player != null)
            {
                AudioManager._instance.PlaySFX("DestructibleObject", 1); // Memainkan suara efek saat orb diambil
                player.RegenHPOrb(hpAmount); // Panggil method RegenHPOrb pada pemain dengan jumlah HP yang sesuai
                player.RegenSPOrb(spAmount); // Panggil method RegenSPOrb pada pemain dengan jumlah SP yang sesuai
                Debug.Log("Player caught HP orb and gained SP!");
            }

            Destroy(gameObject); // Hancurkan objek orb yang diambil
        }
    }
}

