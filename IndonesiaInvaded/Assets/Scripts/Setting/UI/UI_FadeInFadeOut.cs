using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeInFadeOut : MonoBehaviour
{
    [SerializeField] private CanvasGroup UIGroup;

    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;
    // Start is called before the first frame update

    public static UI_FadeInFadeOut Instance { get; private set; }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(fadeIn)
        {
            if(UIGroup.alpha < 1)
            {
                UIGroup.alpha += Time.deltaTime;
                if(UIGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
            
        }
        if(fadeOut)
        {
            if(UIGroup.alpha > 0)
            {
                UIGroup.alpha -= Time.deltaTime;
                if(UIGroup.alpha <= 0)
                {
                    fadeOut = false;
                }
            }
        }
        
    }

    public void ShowUI()
    {
        fadeIn = true;
    }

    public void HideUI()
    {
        fadeOut = true;
    }


}
