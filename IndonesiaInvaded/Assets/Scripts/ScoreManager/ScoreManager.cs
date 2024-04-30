using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score = 0;

    public int enemyDefeats;

    public TMP_Text scoreText; // Referensi ke UI Text untuk menampilkan skor

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // Pastikan scoreText tidak null
        if (scoreText == null)
        {
            Debug.LogError("UI Text untuk menampilkan skor belum ditetapkan!");
        }
        else
        {
            // Perbarui tampilan skor awal
            UpdateScoreText();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
        
        // Perbarui tampilan skor setelah menambah skor
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        // Perbarui teks UI Text dengan nilai skor yang baru
        scoreText.text = "Score: " + score;
    }

    public void AddEnemyDefeats(int amount)
    {
        enemyDefeats += amount;
        Debug.Log("Enemy Defeats: " + enemyDefeats);
    }
}
