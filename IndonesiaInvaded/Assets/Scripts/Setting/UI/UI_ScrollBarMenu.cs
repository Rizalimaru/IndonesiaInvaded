using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ScrollBarMenu : MonoBehaviour, IPointerEnterHandler
{
    public Scrollbar scrollBar;
    public TMP_Text scoreText;
    public TMP_Text rankText;
    public Button[] buttons;
    float[] pos;
    float distance;
    int hoveredIndex = -1;

    void Start()
    {
        pos = new float[buttons.Length];
        distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        UpdateTexts(0);
    }

    void Update()
    {
        if (hoveredIndex >= 0)
        {
            float normalizedScrollPos = scrollBar.value;
            int currentIndex = Mathf.RoundToInt(normalizedScrollPos / distance);

            if (currentIndex != hoveredIndex)
            {
                UpdateTexts(hoveredIndex);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>())
            {
                hoveredIndex = i;
                UpdateTexts(i);
                break;
            }
        }
    }

    void UpdateTexts(int index)
    {
        scoreText.text = GetScore(index).ToString();
        rankText.text = GetRank(index);
    }

    int GetScore(int index)
    {
        return index * 100;
    }

    string GetRank(int index)
    {
        switch (index)
        {
            case 0:
                return "A+";
            case 1:
                return "A";
            case 2:
                return "B";
            case 3:
                return "C";
            default:
                return "D";
        }
    }
}