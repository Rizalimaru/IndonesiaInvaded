using System.Collections;
using UnityEngine;


public class UI_AnimatorUI : MonoBehaviour
{
    public static UI_AnimatorUI instance { get; set;}

    public Animator animator;


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
        animator.SetTrigger("FadeOut");
    }
    
}
