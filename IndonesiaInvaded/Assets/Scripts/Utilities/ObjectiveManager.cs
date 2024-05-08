using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject objectiveUIPanel;
    public TMP_Text objectiveUIText;
    public int targetEnemyCount = 2;
    private int killedEnemyCount = 0; // Mengganti destroyedEnemyCount menjadi killedEnemyCount
    private bool objectiveActive = false;
    private bool objectiveCompleted = false;

    private Collider objectiveCollider;

    private void Start()
    {
        // Dapatkan collider dari game object ini
        objectiveCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !objectiveCompleted)
        {
            StartObjective();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && objectiveCompleted)
        {
            EndObjective();
        }
    }

    private void StartObjective()
    {
        objectiveActive = true;
        objectiveUIPanel.SetActive(true);
        objectiveUIText.text = "Defeat enemies (0/" + targetEnemyCount + ")";
    }

    private void EndObjective()
    {
        objectiveActive = false;
        objectiveUIPanel.SetActive(false);
        objectiveUIText.text = "";
        Destroy(objectiveCollider);
        Destroy(this);
    }

    // Mengubah DestroyEnemy menjadi EnemyKilled
    public void EnemyKilled()
    {
        if (objectiveActive)
        {
            killedEnemyCount++; // Mengubah destroyedEnemyCount menjadi killedEnemyCount
            objectiveUIText.text = "Defeat enemies (" + killedEnemyCount + "/" + targetEnemyCount + ")";

            if (killedEnemyCount >= targetEnemyCount && !objectiveCompleted)
            {
                CompleteObjective();
            }
        }
    }

    private void CompleteObjective()
    {
        objectiveActive = false;
        objectiveCompleted = true;
        Debug.Log("Objective completed!");
    }
}
