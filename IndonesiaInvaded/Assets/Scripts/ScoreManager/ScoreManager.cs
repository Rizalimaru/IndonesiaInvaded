using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Value")]
    public int score;
    public int enemyDefeats;
    public int bossDefeats;
    public float time;

    [Header("UI Text")]
    public TMP_Text timeTextInPaused; 
    public TMP_Text scoreTextInGame; 
    public TMP_Text scoreTextPaused; 
    
    [Header("Animation")]
    private Animator scoreAnimator;

    [Header("Private Variable and Haven't been used yet")]
    private Coroutine hideScoreTextCoroutine;
    private float hideDelay = 10f; // Delay sebelum teks skor di game di hide
    public int bonus { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        // Ambil komponen Animator dari UI Text skor di game
        scoreAnimator = scoreTextInGame.GetComponent<Animator>();
        // Sembunyikan teks skor di game saat memulai permainan
        scoreTextInGame.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Update waktu
        time += Time.deltaTime;

        //Update score
        scoreTextPaused.text = score.ToString();

        // Tampilkan waktu di pause menu
        DisplayTime(time);

        // Hitung bonus berdasarkan waktu
        DetermineBonus();
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay +=1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeTextInPaused.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Restart Semua Nilai
    public void ResetAllValues()
    {
        score = 0;
        enemyDefeats = 0;
        bossDefeats = 0;
        time = 0;
    }
    
    private void DetermineBonus()
    {
        if (time < 240) // Kurang dari 4 menit (240 detik)
        {
            bonus = 10000;
        }
        else if (time < 300) // Kurang dari 5 menit (300 detik)
        {
            bonus = 8000;
        }
        else if (time < 360) // Kurang dari 6 menit (360 detik)
        {
            bonus = 7000;
        }
        else // Lebih dari 7 menit (420 detik)
        {
            bonus = 5000;
        }
    }
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
        
        // Perbarui tampilan skor setelah menambah skor
        UpdateScoreTextInGame();
        UpdateScoreTextPaused();

        // Reset delay untuk hide teks skor di game
        if (hideScoreTextCoroutine != null)
        {
            StopCoroutine(hideScoreTextCoroutine);
        }
        hideScoreTextCoroutine = StartCoroutine(HideScoreTextAfterDelay());
    }
    private IEnumerator HideScoreTextAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);

        scoreAnimator.SetTrigger("hide");

        yield return new WaitForSeconds(1);
        // Hide teks skor di game setelah delay
        scoreTextInGame.gameObject.SetActive(false);
        
    }

    private void UpdateScoreTextInGame()
    {
        // Perbarui teks UI Text di game dengan nilai skor yang baru
        scoreTextInGame.text = score.ToString();

        // Tampilkan kembali teks skor di game
        scoreTextInGame.gameObject.SetActive(true);
    }

    private void UpdateScoreTextPaused()
    {
        // Perbarui teks UI Text di pause menu dengan nilai skor yang baru
        scoreTextPaused.text = score.ToString();
    }

    public void AddEnemyDefeats(int amount)
    {
        enemyDefeats += amount;
        Debug.Log("Enemy Defeats: " + enemyDefeats);
    }

    public void AddBossDefeats(int amount)
    {
        bossDefeats += amount;
        Debug.Log("Boss Defeats: " + bossDefeats);
    }
}
