using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
   [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    
    [Header("Button UI")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;
    

    private void Start(){
        DisableButtonsDependingOnData();
    }

    private void DisableButtonsDependingOnData() 
    {
        if (!GameManager.instance.HasGameData()) 
        {
            loadButton.interactable = false;
        }
    }
    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }


    public void OnLoadGameClicked(){
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnOptionsClicked()
    {
        Debug.Log("Options Clicked");
    }

    public void OnExitClicked()
    {
        Debug.Log("Exit Clicked");
    }

    public void ActivateMenu(){
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }

    public void DeactivateMenu(){
        this.gameObject.SetActive(false);
    }
}
