using UnityEngine;

[CreateAssetMenu(fileName = "Boss Configuration", menuName = "ScriptableObject/Boss Configuration")]
public class BossScriptableObject : ScriptableObject
{
    // Attribute Config
    public float Health = 500f;
    public float attackPower = 10f;
    public float attackSpeed = 1.0f;
    public float triggerDistance = 5f;
    public float attackDistance = 1f;
    public float viewingAngle = 30f;
    public float knockbackGuard = 30f;

    // Skill Config
    public float firstSkillChance = 50f;
    public float secondSkillChance = 10f;
    public float firstSkillAnimDelay = 2.5f;
    public float secondSkillAnimDelay = 2.5f;

    // NavMesh Config = Steering
    public float Speed = 10;
    public float AngularSpeed = 240;
    public float Acceleration = 10;
    public float StoppingDistance = 0;
    public bool Braking = true;

    // Boss Type
    public title bossTitle = title.OndelOndel;

    // Animation Purpose
    public float animationDelay = 1;

    public enum title
    {
        OndelOndel,
        Dukun
    }
}
