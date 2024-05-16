using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class UI_ScrollBarMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDataPersistence
{
    public Scrollbar scrollBar;
    public TMP_Text scoreText;
    public TMP_Text rankText;
    public Button[] buttons;
    float[] pos;
    float distance;
    int hoveredIndex = -1;

    public Dictionary<string, GameData> scoreAndRank;
    public int highScore;
    public string rank;
    public string selectedProfileId = "";
    private LevelCheck[] levelCheck;
    private GameData data;

    private void Awake()
    {
        levelCheck = this.GetComponentsInChildren<LevelCheck>();
    }
    private void Start()
    {
        data = new GameData();
        scoreAndRank = new Dictionary<string, GameData>();
        pos = new float[buttons.Length];
        distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        InitializeSelectedProfileId();
        GameManager.instance.LoadGame();
        Debug.Log(selectedProfileId);
    }

    private void Update()
    {
        if (hoveredIndex >= 0)
        {
            float normalizedScrollPos = scrollBar.value;
            int currentIndex = Mathf.RoundToInt(normalizedScrollPos / distance);

            if (currentIndex != hoveredIndex)
            {
                UpdateTexts();
            }
        }
    }
    private void InitializeSelectedProfileId()
    {
        string selectedProfileId = "";
        int highestScoreIndex = -1;

        for (int i = 0; i < levelCheck.Length; i++)
        {
            string profileId = levelCheck[i].GetProfileId();
            GameData profileData = data.playerData[profileId];

            if (int.TryParse(profileData.highScore.ToString(), out int score) && int.TryParse(data.highScore.ToString(), out int currentScore))
            {
                if (score > currentScore)
                {
                    highestScoreIndex = i;
                }
            }

            if (selectedProfileId == "")
            {
                selectedProfileId = profileId;
            }
        }

        if (highestScoreIndex >= 0)
        {
            selectedProfileId = levelCheck[highestScoreIndex].GetProfileId();
        }

        GameManager.instance.ChangeSelectedProfileId(selectedProfileId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>())
            {
                hoveredIndex = i;
                UpdateTexts();
                break;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset hoveredIndex when pointer exits
        hoveredIndex = -1;
    }

    public void UpdateTexts()
    {

        // scoreText.text = highScore.ToString();
        // rankText.text = rank;
        if (scoreAndRank.TryGetValue(selectedProfileId, out GameData playerData))
        {
            scoreText.text = playerData.highScore.ToString();
            rankText.text = playerData.rank;
        }
    }

    private void GetProfileId(string profileId)
    {
        GameManager.instance.ChangeSelectedProfileId(profileId);
    }

    public void SaveData(GameData data)
    {
        data.playerData[selectedProfileId] = new GameData { highScore = highScore, rank = rank };
    }

    public void LoadData(GameData data)
    {
        highScore = data.highScore;
        rank = data.rank;

        if (data.playerData.TryGetValue(selectedProfileId, out GameData profile))
        {
            highScore = profile.highScore;
            rank = profile.rank;
        }
    }
}
