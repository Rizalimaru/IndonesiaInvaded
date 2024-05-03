using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score;

    public int enemyDefeats;

    public int bossDefeats;

    [Header("UI Text")]

    public TMP_Text scoreTextInGame; // Referensi ke UI Text untuk menampilkan skor
    public TMP_Text scoreTextPaused; // Referensi ke UI Text lain untuk menampilkan skor

    public TMP_Text timeText; // Referensi ke UI Text untuk menampilkan waktu

    public int bonus { get; private set; }
    
    [Header("Animation")]
    private Animator scoreAnimator;



    private Coroutine hideScoreTextCoroutine;
    private float hideDelay = 10f; // Delay sebelum teks skor di game di hide

    public float timeValue 
    {
        get { return float.Parse(timeText.text.Split(' ')[0]); }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        // Update waktu
        timeText.text = Time.time.ToString("F0") + " S";

        DetermineBonus();

    }

    private void Start()
    {
        // Ambil komponen Animator dari UI Text skor di game
        scoreAnimator = scoreTextInGame.GetComponent<Animator>();
        // Sembunyikan teks skor di game saat memulai permainan
        scoreTextInGame.gameObject.SetActive(false);
    }

    public void ResetTime()
    {
        timeText.text = "0 S";
    }

    private void DetermineBonus()
    {
        float timeCompleted = timeValue; // Waktu selesai permainan dalam detik

        if (timeCompleted < 240) // Kurang dari 4 menit (240 detik)
        {
            bonus = 10000;
        }
        else if (timeCompleted < 300) // Kurang dari 5 menit (300 detik)
        {
            bonus = 8000;
        }
        else if (timeCompleted < 360) // Kurang dari 6 menit (360 detik)
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
