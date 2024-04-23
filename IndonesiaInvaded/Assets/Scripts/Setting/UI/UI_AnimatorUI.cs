using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_AnimatorUI : MonoBehaviour
{
    public static UI_AnimatorUI instance;

    private Animator animator;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            animator = GetComponent<Animator>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void LoadGameAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("FadeOut");
        }
    }
}
