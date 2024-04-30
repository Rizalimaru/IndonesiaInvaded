using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttributeData
{
    public int vitality;
    public int strength;
    public int intellect;
    public int endurance;

    public AttributeData() 
    {
        this.vitality = 1;
        this.strength = 1;
        this.intellect = 1;
        this.endurance = 1;
    }
}
