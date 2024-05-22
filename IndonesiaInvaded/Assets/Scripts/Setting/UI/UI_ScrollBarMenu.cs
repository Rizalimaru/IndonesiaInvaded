using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UI_ScrollBarMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDataPersistence
{
    public Scrollbar scrollBar;
    public TMP_Text scoreText;
    public TMP_Text rankText;
    public Button[] buttons;
    float[] pos;
    float distance;
    int hoveredIndex = -1;
    private Dictionary<string, GameData> allProfilesData = new Dictionary<string, GameData>();
    private string currentProfileId;

    private int highScore;
    private string rank;
    private string selectedProfileId;

    private void Start()
    {
        pos = new float[buttons.Length];
        distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        GameManager.instance.UpdateAllProfilesData();
    }

    private void Update()
    {
        UpdateProfileData(selectedProfileId);
        if (hoveredIndex >= 0 && hoveredIndex < buttons.Length)
        {
            float normalizedScrollPos = scrollBar.value;
            Mathf.RoundToInt(normalizedScrollPos / distance);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>())
            {
                hoveredIndex = i;
                string newProfileId = buttons[i].GetComponent<LevelCheck>().GetProfileId();
                GameManager.instance.UpdateProfileData(newProfileId);
                UpdateTexts(hoveredIndex);
                break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoveredIndex = -1;
    }

    public void UpdateTexts(int hoveredIndex)
    {
        if (hoveredIndex >= 0 && hoveredIndex < buttons.Length)
        {
            string profileId = buttons[hoveredIndex].GetComponent<LevelCheck>().GetProfileId();
            if (allProfilesData.ContainsKey(profileId))
            {
                scoreText.text = allProfilesData[profileId].highScore.ToString();
                rankText.text = allProfilesData[profileId].rank;
            }
            else
            {
                // Jika data profil tidak ada, kosongkan teks
                scoreText.text = "";
                rankText.text = "";
            }
        }
    }

    public void SaveData(GameData data)
    {
        // Implementasi logika penyimpanan jika diperlukan
    }

    public void LoadData(GameData data)
    {
        // Implementasi logika pemuatan jika diperlukan
    }

    public void UpdateProfileData(string newProfileId)
    {
        selectedProfileId = newProfileId;
        GameManager.instance.LoadGame(); // Memuat kembali data game dengan profil yang baru
        // Panggil UpdateTexts untuk menampilkan data baru
        if (hoveredIndex >= 0 && hoveredIndex < buttons.Length)
        {
            UpdateTexts(hoveredIndex);
        }
    }

    public void UpdateProfileData(Dictionary<string, GameData> allProfilesData, string currentProfileId)
    {
        this.allProfilesData = allProfilesData;
        this.currentProfileId = currentProfileId;
    }
}
