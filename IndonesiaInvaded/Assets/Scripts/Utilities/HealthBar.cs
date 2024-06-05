using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Image profileImage; // Reference to the Image UI component for player's profile picture
    public Sprite normalProfileSprite; // Profile image when health is above 30%
    public Sprite lowHealthProfileSprite; // Profile image when health is 30% or below

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        UpdateProfileImage(health);
    }

    private void UpdateProfileImage(int health)
    {
        // Calculate the threshold value for 30% health
        float threshold = slider.maxValue * 0.3f;
        if (health <= threshold)
        {
            profileImage.sprite = lowHealthProfileSprite;
        }
        else
        {
            profileImage.sprite = normalProfileSprite;
        }
    }
}
