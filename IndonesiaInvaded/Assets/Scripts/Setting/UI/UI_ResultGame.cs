using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UI_ResultGame : MonoBehaviour
{
    public static UI_ResultGame instance;
    public Image scoreImage;
    public Sprite spriteD, spriteC, spriteB, spriteA, spriteS;

    public TMP_Text scoreText;
    public TMP_Text enemyDefeatsText;
    public TMP_Text bossDefeatsText;
    public TMP_Text bonusText;
    public int maxScore;

    public TMP_Text totalScoreText; // Teks untuk menampilkan total skor

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = ScoreManager.instance;

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        // Ambil nilai skor dan jumlah musuh yang dikalahkan dari ScoreManager
        int score = scoreManager.score;
        int enemyDefeats = scoreManager.enemyDefeats;

        // Tampilkan nilai skor dan jumlah musuh yang dikalahkan di UI Result


        // Hitung total skor berdasarkan skor, jumlah musuh yang dikalahkan, jumlah boss yang dikalahkan, dan bonus
        int totalScore = CalculateTotalScore(score, enemyDefeats, 0, 0);

        // Tampilkan teks total skor

        // Ubah sprite score berdasarkan nilai total skor
        if (totalScore >= maxScore * 0.9f)      // S
        {
            scoreImage.sprite = spriteS;
        }
        else if (totalScore >= maxScore * 0.8f) // A
        {
            scoreImage.sprite = spriteA;
        }
        else if (totalScore >= maxScore * 0.7f) // B
        {
            scoreImage.sprite = spriteB;
        }
        else if (totalScore >= maxScore * 0.6f) // C
        {
            scoreImage.sprite = spriteC;
        }
        else                                    // D
        {
            scoreImage.sprite = spriteD;
        }
    }

    // Metode untuk menghitung total skor berdasarkan skor, jumlah musuh yang dikalahkan, jumlah boss yang dikalahkan, dan bonus
    private int CalculateTotalScore(int score, int enemyDefeats, int bossDefeats, int bonus)
    {
        // Contoh: total skor dihitung berdasarkan skor, jumlah musuh yang dikalahkan, jumlah boss yang dikalahkan, dan bonus
        int totalScore = score + (enemyDefeats * 1000) + (bossDefeats * 500) + bonus;
        return totalScore;
    }

    public void ShowResult()
    {
        // Memulai animasi perhitungan skor
        StartCoroutine(AnimateScore());
    }

    private IEnumerator AnimateScore()
    {
        int currentScore = 0;

        // Animasi perhitungan skor
        while (currentScore < scoreManager.score)
        {
            currentScore += 10; // Penambahan skor per frame (sesuaikan dengan kebutuhan)
            if (currentScore > scoreManager.score)
                currentScore = scoreManager.score; // Pastikan nilai skor tidak melampaui nilai aktual

            scoreText.text = currentScore.ToString();

            yield return null; // Tunggu satu frame
        }

        int currentEnemyDefeats = 0;

        // Animasi perhitungan jumlah musuh yang dikalahkan
        while (currentEnemyDefeats < scoreManager.enemyDefeats)
        {
            currentEnemyDefeats += 1; // Penambahan jumlah musuh yang dikalahkan per frame (sesuaikan dengan kebutuhan)
            if (currentEnemyDefeats > scoreManager.enemyDefeats)
                currentEnemyDefeats = scoreManager.enemyDefeats; // Pastikan nilai jumlah musuh yang dikalahkan tidak melampaui nilai aktual

            enemyDefeatsText.text = currentEnemyDefeats.ToString();

            yield return null; // Tunggu satu frame
        }

        int currentTotalScore = 0;

        // Animasi perhitungan total skor
        while (currentTotalScore < CalculateTotalScore(scoreManager.score, scoreManager.enemyDefeats, 0, 0))
        {
            currentTotalScore += 50; // Penambahan total skor per frame (sesuaikan dengan kebutuhan)
            if (currentTotalScore > CalculateTotalScore(scoreManager.score, scoreManager.enemyDefeats, 0, 0))
                currentTotalScore = CalculateTotalScore(scoreManager.score, scoreManager.enemyDefeats, 0, 0); // Pastikan nilai total skor tidak melampaui nilai aktual

            totalScoreText.text = currentTotalScore.ToString();

            yield return null; // Tunggu satu frame
        }

        scoreImage.gameObject.SetActive(true);
    }
}
