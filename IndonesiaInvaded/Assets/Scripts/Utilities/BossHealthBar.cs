using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{

    public static BossHealthBar instance;
    public Slider healthSlider;
    public Image fillImage;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }
    public void Initialize(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void UpdateHealthBar(float currentHealth)
    {
        healthSlider.value = currentHealth;
    }
}
