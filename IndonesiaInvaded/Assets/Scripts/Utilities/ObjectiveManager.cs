using System.Collections;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    public GameObject objectiveUIPanel;
    public TextMeshProUGUI objectiveUIText;
    private int killedEnemyCount = 0;
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
        if (GameObject.FindWithTag("ObjectiveUI") != null)
        {
            objectiveUIPanel = GameObject.FindWithTag("ObjectiveUI").GetComponent<GameObject>();
        }
            
    }

    private int GetCurrentEnemy()
    {
        int enemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        return enemy;
    }


    // Menampilkan UI objektif dengan teks yang sudah ditentukan
    public void ShowObjective()
    {
        if (objectiveUIPanel != null) 
        {
            objectiveUIPanel.SetActive(true);
            objectiveUIText.text = "Defeat enemies (0/" + GetCurrentEnemy() + ")";
        }
        
    }

    // Menyembunyikan UI objektif
    public void HideObjective()
    {
        objectiveUIPanel.SetActive(false);
    }

    // Memulai objektif dan spawning musuh
    public void StartObjective()
    {
        ShowObjective();
        Debug.Log("UI Terbuka");
    }

    // Memperbarui UI objektif dengan jumlah musuh yang dibunuh
    public void UpdateObjective()
    {
        killedEnemyCount++;
        objectiveUIText.text = "Defeat enemies (" + killedEnemyCount + "/" + GetCurrentEnemy() + ")";

        if (killedEnemyCount >= GetCurrentEnemy() && !objectiveCompleted)
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
        HideObjective();
        objectiveCompleted = false; 
    }


}
