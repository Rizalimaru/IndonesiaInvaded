using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class CreditSceneController : MonoBehaviour
{
    public Button backmainmenu;

    public Animator creditAnimator;

    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        creditAnimator.SetTrigger("hidecredit");
        StartCoroutine(DelayHideCredit());
       // Kembali ke main menu
    }

    IEnumerator DelayHideCredit()
    {
        yield return new WaitForSeconds(2f);
        Scene_Loading.instance.LoadMainMenu();
    }
}
