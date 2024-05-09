using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCheck : MonoBehaviour
{
    [Header("Level")]
    public int levelNumber;
    public GameObject levelLocked;
    public GameObject levelUnlock;
    private void Start()
    {
        if (!GameManager.instance.IsLevelUnlocked(levelNumber))
        {
            LockLevel();
        }
    }

    private void LockLevel()
    {
        levelLocked.SetActive(true);
        levelUnlock.SetActive(false);
        GetComponent<Button>().interactable = false;
    }
}
