using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_EnemyAudioControl : MonoBehaviour
{
    private AudioSource enemyAudioSource;
    private bool isAudioMuted = false;

    private void Start()
    {
        // Mengambil komponen AudioSource pada objek ini
        enemyAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Memeriksa apakah permainan sedang dalam keadaan pause
        if (Time.timeScale == 0f && !isAudioMuted)
        {
            // Jika dalam keadaan pause dan audio belum dimute, mute audio
            enemyAudioSource.Stop();
            isAudioMuted = true;
        }
        else if (Time.timeScale != 0f && isAudioMuted)
        {
            // Jika tidak dalam keadaan pause dan audio sedang dimute, unmute audio
            enemyAudioSource.Play();
            isAudioMuted = false;
        }
    }

}
