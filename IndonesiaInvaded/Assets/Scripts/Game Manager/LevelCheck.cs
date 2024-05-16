using UnityEngine;
using UnityEngine.UI;

public class LevelCheck : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Level")]
    public int levelNumber;

    [Header("Game Object")]
    public GameObject levelLocked;
    public GameObject levelUnlock;

    public bool hasData { get; private set; } = false;

    private void Start()
    {
        LevelManager levelManager = LevelManager.instance;
        if (levelManager != null)
        {
            if (!levelManager.IsLevelUnlocked(levelNumber))
            {

                LockLevel();
            }
            else
            {
                LoadLevel();
            }
        }
        else
        {
            Debug.LogError("levelManager instance is null. Cannot load level data.");
        }
    }
    private void LockLevel()
    {
        levelLocked.SetActive(true);
        levelUnlock.SetActive(false);
        GetComponent<Button>().interactable = false;
    }

    private void LoadLevel()
    {
        levelLocked.SetActive(false);
        levelUnlock.SetActive(true);
        GetComponent<Button>().interactable = true;
    }

    public string GetProfileId()
    {
        return this.profileId;
    }

}
