using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    // Attribute Config
    public float Health = 100f;
    public float attackPower = 10f;
    public float attackSpeed = 1.0f;
    public float triggerDistance = 5f;
    public float attackDistance = 1f;
    public float viewingAngle = 30f;
    public float knockbackGuard = 15f;

    // NavMesh Config = Steering
    public float Speed = 3;
    public float AngularSpeed = 120;
    public float Acceleration = 8;
    public float StoppingDistance = 0;
    public bool Braking = true;

    // Enemy Type (Boss, Basic)
    public title enemyTitle = title.Basic_Melee;

    // Animation Purpose
    public float animationDelay = 1;

    public enum title
    {
        Basic_Melee,
        Basic_Ranged,
    }
}
