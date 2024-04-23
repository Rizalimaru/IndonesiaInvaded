using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ResultGame : MonoBehaviour
{
    public Image scoreImage;
    public Sprite spriteD, spriteC, spriteB, spriteA, spriteS;

    public TMP_Text scoreText;
    public TMP_Text enemyDefeatsText;
    public TMP_Text bossDefeatsText;
    public TMP_Text bonusText;
    public int maxScore;

    public TMP_Text totalScoreText; // Teks untuk menampilkan total skor

    private void Update()
    {
        // Ambil nilai skor, jumlah musuh yang dikalahkan, jumlah boss yang dikalahkan, dan bonus dari teks pada objek UI
        int score = int.Parse(scoreText.text);
        int enemyDefeats = int.Parse(enemyDefeatsText.text);
        int bossDefeats = int.Parse(bossDefeatsText.text);
        int bonus = int.Parse(bonusText.text);

        // Hitung total skor berdasarkan skor, jumlah musuh yang dikalahkan, jumlah boss yang dikalahkan, dan bonus
        int totalScore = CalculateTotalScore(score, enemyDefeats, bossDefeats, bonus);

        // Tampilkan teks total skor
        totalScoreText.text = totalScore.ToString();


        // Ubah sprite score berdasarkan nilai total skor
        if (totalScore >= maxScore * 0.9f)      // S
        {
            scoreImage.sprite = spriteS;
            Debug.Log("S");
        }
        else if (totalScore >= maxScore * 0.8f) // A
        {
            scoreImage.sprite = spriteA;
            Debug.Log("A");
        }
        else if (totalScore >= maxScore * 0.7f) // B
        {
            scoreImage.sprite = spriteB;
            Debug.Log("B");
        }
        else if (totalScore >= maxScore * 0.6f) // C
        {
            scoreImage.sprite = spriteC;
            Debug.Log("C");
        }
        else                                    // D
        {
            scoreImage.sprite = spriteD;
            Debug.Log("D");
        }
    }

    // Metode untuk menghitung total skor berdasarkan skor, jumlah musuh yang dikalahkan, jumlah boss yang dikalahkan, dan bonus
    private int CalculateTotalScore(int score, int enemyDefeats, int bossDefeats, int bonus)
    {
        // Contoh: total skor dihitung berdasarkan skor, jumlah musuh yang dikalahkan, jumlah boss yang dikalahkan, dan bonus
        int totalScore = score + (enemyDefeats * 100) + (bossDefeats * 500) + bonus;
        return totalScore;
    }
}
