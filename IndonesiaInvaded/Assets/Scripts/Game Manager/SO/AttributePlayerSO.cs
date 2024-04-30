using UnityEngine;

[CreateAssetMenu(fileName = "Attributes", menuName = "ScriptableObjects/AttributePlayerSO", order = 1)]
public class AttributePlayerSO : ScriptableObject
{
    public int vitality;
    public int strength;
    public int intellect;
    public int endurance;
}
