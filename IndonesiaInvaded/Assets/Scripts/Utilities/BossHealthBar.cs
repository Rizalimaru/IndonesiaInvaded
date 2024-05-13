using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Image fillImage;

    private EnemyScriptableObject enemyConfig;
    private float maxHealth;

    private void Start()
    {
        // Dapatkan referensi ke EnemyScriptableObject dari boss GameObject
        enemyConfig = GetComponent<Enemy>().enemyConfiguration;

        // Atur nilai maksimum HP sesuai dengan nilai yang ada di EnemyScriptableObject
        maxHealth = enemyConfig.Health;

        // Atur nilai maksimum slider HP
        healthSlider.maxValue = maxHealth;

        // Perbarui UI HP bar untuk pertama kalinya
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        // Dapatkan nilai kesehatan saat ini dari EnemyScriptableObject
        float currentHealth = enemyConfig.Health;

        // Atur nilai slider sesuai dengan kesehatan saat ini
        healthSlider.value = currentHealth;

        // Hitung persentase kesehatan saat ini
        float healthPercentage = currentHealth / maxHealth;

        // Atur ukuran fill image sesuai dengan persentase kesehatan
        fillImage.fillAmount = healthPercentage;
    }
}
