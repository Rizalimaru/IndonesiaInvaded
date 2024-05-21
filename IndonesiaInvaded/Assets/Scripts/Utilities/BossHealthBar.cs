using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Image fillImage;
    public Boss boss; // Referensi ke Boss

    private void Start()
    {
        if (boss != null)
        {
            Debug.Log("BossHealthBar: Boss ditemukan");
            Initialize(boss.health);
        }
        else
        {
            Debug.LogError("BossHealthBar: Boss tidak ditemukan");
        }
    }

    public void Initialize(float maxHealth)
    {
        Debug.Log("Initializing health bar with max health: " + maxHealth);
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        Debug.Log("Slider max value set to: " + healthSlider.maxValue);
        Debug.Log("Slider current value set to: " + healthSlider.value);
    }

    public void UpdateHealthBar(float currentHealth)
    {
        Debug.Log("Updating health bar with current health: " + currentHealth);
        healthSlider.value = currentHealth;
        Debug.Log("Slider value is now: " + healthSlider.value);
    }
}
