using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour, IDataPersistence
{
    public static ScoreManager instance;

    [Header("Level")]
    public int levelIndex;

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

    private Coroutine hideScoreTextCoroutine;
    private float hideDelay = 10f;
    public int bonus { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        scoreAnimator = scoreTextInGame.GetComponent<Animator>();
        scoreTextInGame.gameObject.SetActive(false);
    }

    public void SaveData(GameData data)
    {
        // data.score = score;
        // data.enemyDefeats = enemyDefeats;
        // data.bossDefeats = bossDefeats;
        // data.time = time;
        // data.bonus = bonus;
        // data.totalScore = data.CalculateTotalScore();
        // data.UpdateHighScore();
        // data.UpdateRank();

        LevelData levelData = data.levelData.Find(x => x.levelNumber == levelIndex);
        if (levelData != null)
        {
            // Menyimpan nilai dari ScoreManager ke dalam data level
            levelData.score = score;
            levelData.enemyDefeats = enemyDefeats;
            levelData.bossDefeats = bossDefeats;
            levelData.time = time;
            levelData.bonus = bonus;
            // Mengupdate totalScore, highScore, dan rank
            levelData.totalScore = levelData.CalculateTotalScore();
            levelData.UpdateHighScore();
            levelData.UpdateRank();
        }

    }

    public void LoadData(GameData data)
    {
        // score = data.score;
        // enemyDefeats = data.enemyDefeats;
        // bossDefeats = data.bossDefeats;
        // time = data.time;
        // bonus = data.bonus;

        LevelData levelData = data.levelData.Find(x => x.levelNumber == levelIndex);
        if (levelData != null)
        {
            // Memuat nilai dari data level ke dalam ScoreManager
            score = levelData.score;
            enemyDefeats = levelData.enemyDefeats;
            bossDefeats = levelData.bossDefeats;
            time = levelData.time;
            bonus = levelData.bonus;
        }
    }

    private void Update()
    {
        UpdateTime();
        UpdateScoreText();
        DisplayTime(time);
        DetermineBonus();
    }

    private void UpdateTime()
    {
        time += Time.deltaTime;
    }

    private void UpdateScoreText()
    {
        scoreTextPaused.text = score.ToString();
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeTextInPaused.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void DetermineBonus()
    {
        if (time < 240)
        {
            bonus = 10000;
        }
        else if (time < 300)
        {
            bonus = 8000;
        }
        else if (time < 360)
        {
            bonus = 7000;
        }
        else
        {
            bonus = 5000;
        }
    }

    public void ResetAllValues()
    {
        score = enemyDefeats = bossDefeats = 0;
        time = 0;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreTextInGame();
        UpdateScoreTextPaused();

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
        scoreTextInGame.gameObject.SetActive(false);
    }

    private void UpdateScoreTextInGame()
    {
        scoreTextInGame.text = score.ToString();
        scoreTextInGame.gameObject.SetActive(true);
    }

    private void UpdateScoreTextPaused()
    {
        scoreTextPaused.text = score.ToString();
    }

    public void AddEnemyDefeats(int amount)
    {
        enemyDefeats += amount;
    }

    public void AddBossDefeats(int amount)
    {
        bossDefeats += amount;
    }

    public void ResetScore()
    {
        score = 0;
        enemyDefeats = 0;
        bossDefeats = 0;
    }

}
