
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void SetMaxSkill(int skill)
    {
        slider.maxValue = skill;
        slider.value = skill;
    }

    public void SetSkill(int skill)
    {
        slider.value = skill;
    }
}
