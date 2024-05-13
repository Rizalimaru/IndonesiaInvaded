using UnityEngine;

public class UI_SettingDisplay : MonoBehaviour
{
    public GameObject[] settingPanels;

    public GameObject settingMenu;


    private void ShowPanel(int index)
    {
        for (int i = 0; i < settingPanels.Length; i++)
        {
            settingPanels[i].SetActive(i == index);
        }
    }
    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingMenu.activeSelf)
            {
                UI_ControlMainMenu.Instance.ShowMenu();
            }
        }
    }


    

    public void ShowController()
    {
        ShowPanel(0);
    }

    public void ShowDisplay()
    {
        ShowPanel(1);
    }

    public void ShowGraphics()
    {
        ShowPanel(2);
    }

    public void ShowAudio()
    {
        ShowPanel(3);
    }

}