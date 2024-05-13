using System.Collections;
using UnityEngine;
using TMPro;


public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    public GameObject objectiveUIPanel;
    public TextMeshProUGUI objectiveUIText;
    private int killedEnemyCount = 0;
    private int enemyCount = 0;
    private bool objectiveCompleted = false;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
     
    }

    private int GetCurrentEnemy()
    {
        int enemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        return enemy;
    }


    // Menampilkan UI objektif dengan teks yang sudah ditentukan
    public void ShowObjective()
    {
        
        objectiveUIPanel.SetActive(true);
        objectiveUIText.text = "Defeat enemies (0/" + enemyCount + ")";
        
        
    }

    // Menyembunyikan UI objektif
    public void HideObjective()
    {
        objectiveUIPanel.SetActive(false);
    }

    // Memulai objektif dan spawning musuh
    public void StartObjective()
    {   
        objectiveUIPanel.SetActive(true);
        enemyCount = GetCurrentEnemy();
        ShowObjective();
        Debug.Log("UI Terbuka");
    }

    // Memperbarui UI objektif dengan jumlah musuh yang dibunuh
    public void UpdateObjective()
    {
        // Score Manager
        ScoreManager.instance.AddEnemyDefeats(1);

        // Update jumlah musuh yang dibunuh
        killedEnemyCount++;
        objectiveUIText.text = "Defeat enemies (" + killedEnemyCount + "/" + enemyCount + ")";

        if (killedEnemyCount >= enemyCount && !objectiveCompleted)
        {
            objectiveCompleted = true;

            StartCoroutine(HideObjectiveAfterDelay(5f));
            objectiveUIText.text = "All Enemy Killed";
        }
    }

    // Mengakhiri objektif dan menyembunyikan UI objektif
    public void EndObjective()
    {
        HideObjective();
    }

    // Menyembunyikan UI objektif setelah penundaan
    private IEnumerator HideObjectiveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        objectiveUIPanel.SetActive(false);
        objectiveCompleted = true; 
    }


}
