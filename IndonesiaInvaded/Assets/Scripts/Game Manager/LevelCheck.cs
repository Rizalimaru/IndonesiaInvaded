using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCheck : MonoBehaviour
{
    [Header("LeveL")]
    public int levelNumber;
    private void Start()
    {
        if (!GameManager.instance.IsLevelUnlocked(levelNumber))
        {
            LockLevel();
        }
    }

    private void LockLevel()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Button>().interactable = false;
    }
}
