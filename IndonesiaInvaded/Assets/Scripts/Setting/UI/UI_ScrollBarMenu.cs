using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_ScrollBarMenu : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollBar;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text rankText;

    private float[] positions;
    private float distance;

    void Start()
    {
        InitializePositions();
        UpdateTexts(0);
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && scrollBar != null)
        {
            float normalizedScrollPos = scrollBar.value;
            int currentIndex = Mathf.RoundToInt(normalizedScrollPos / distance);
            UpdateTexts(currentIndex);
        }
    }

    void UpdateTexts(int index)
    {
        if (scoreText != null && rankText != null)
        {
            scoreText.text = GetScore(index).ToString();
            rankText.text = GetRank(index);
        }
    }

    int GetScore(int index)
    {
        return index * 100;
    }

    string GetRank(int index)
    {
        return "A+";
    }

    void InitializePositions()
    {
        int childCount = transform.childCount;
        positions = new float[childCount];
        distance = 1f / (childCount - 1f);
        for (int i = 0; i < childCount; i++)
        {
            positions[i] = distance * i;
        }
    }
}