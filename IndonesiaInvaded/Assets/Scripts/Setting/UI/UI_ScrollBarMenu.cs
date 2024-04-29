using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UI_ScrollBarMenu : MonoBehaviour
{
    public Scrollbar scrollBar;
    public TMP_Text scoreText;
    public TMP_Text rankText;
    float[] pos;
    float distance;

    void Start()
    {
        pos = new float[transform.childCount];
        distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        // Inisialisasi nilai teks dan skor untuk posisi awal scroll
        UpdateTexts(0);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Hitung indeks posisi scroll saat ini
            float normalizedScrollPos = scrollBar.value;
            int currentIndex = Mathf.RoundToInt(normalizedScrollPos / distance);

            // Update teks dan skor berdasarkan indeks saat ini
            UpdateTexts(currentIndex);
        }
    }

    // Fungsi untuk memperbarui teks dan skor berdasarkan indeks
    void UpdateTexts(int index)
    {
        // Contoh implementasi: Tampilkan teks dan skor berdasarkan indeks
        scoreText.text =  GetScore(index).ToString();
        rankText.text = GetRank(index).ToString();
    }

    // Fungsi untuk mendapatkan skor berdasarkan indeks
    int GetScore(int index)
    {

        // Contoh implementasi: Mendapatkan skor berdasarkan indeks
        return index * 100;
    }

    // Fungsi untuk mendapatkan peringkat berdasarkan indeks
    string GetRank(int index)
    {
        // Contoh implementasi: Mendapatkan peringkat berdasarkan indeks
        return "A+";
    }
}
