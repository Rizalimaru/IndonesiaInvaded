using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager instance;
    [SerializeField] private Image fadeOutImage;
    [Range(0.1f,10f), SerializeField] private float fadeOutSpeed = 5f;
    [Range(0.1f,10f), SerializeField] private float fadeInSpeed = 5f;
    [SerializeField] private Color fadeOutStartColor;
    public bool isFadingOut { get; private set; }
    public bool isFadingIn { get; private set; }


    private void Awake(){
        if(instance == null)
        {
            instance = this;
        }
        fadeOutStartColor.a = 0f;
    }

    private void Update()
    {
        if(isFadingOut){
            if(fadeOutImage.color.a <1f){
                fadeOutStartColor.a += Time.deltaTime * fadeOutSpeed;
                fadeOutImage.color = fadeOutStartColor;
            }
            else{
                isFadingOut = false;
            }
        }
        if(isFadingIn){
            if(fadeOutImage.color.a >0f){
                fadeOutStartColor.a -= Time.deltaTime * fadeInSpeed;
                fadeOutImage.color = fadeOutStartColor;
            }
            else{
                isFadingIn = false;
            }   
        }
    }

    public void StartFadeOut(){
        fadeOutImage.color = fadeOutStartColor;
        isFadingOut = true; 
    }

    public void StartFadeIn(){
        if(fadeOutImage.color.a >= 1f){
            fadeOutImage.color = fadeOutStartColor;
            isFadingIn = true; 
        }
    }
}
