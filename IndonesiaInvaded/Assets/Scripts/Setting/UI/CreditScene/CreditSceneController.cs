using UnityEngine;
using UnityEngine.UI;

public class CreditSceneController : MonoBehaviour
{
    public Button backmainmenu;

    // Start is called before the first frame update
    void Start()
    {
        // Pastikan button tidak terlihat di awal
        backmainmenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Memeriksa jika ada klik kiri pada layar
        if (Input.GetMouseButtonDown(0))
        {
            backmainmenu.gameObject.SetActive(true);
        }
    }
    void ShowBackMainMenuButton()
    {
        
    }

    public void BackToMainMenu()
    {
        // Kembali ke main menu
        Scene_Loading.instance.LoadMainMenu();
    }
}
