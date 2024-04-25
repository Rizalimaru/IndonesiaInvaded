using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    // Attribute Config
    public float Health = 100f;
    public float attackPower = 10f;
    public float attackSpeed = 1.0f;
    public float triggerDistance = 5f;
    public float attackDistance = 1f;
    public float attackForce = 1000f;
    public float attackDecay = 0.1f; // Dictate the life of attack collider
    public float viewingAngle = 30f;

    // NavMesh Config = Steering
    public float Speed = 3;
    public float AngularSpeed = 120;
    public float Acceleration = 8;
    public float StoppingDistance = 0;
    public bool Braking = true;

    // NavMesh Config = Obstacle Avoidance
    public ObstacleAvoidanceType ObstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    public float Radius = 0.5f;
    public float Height = 2;
    public int AvoidancePriority = 50;

    // NavMesh Config = Path Finding
    public int AreaMask = -1; // -1 means everything

    // Enemy Type (Boss, Basic)
    public title enemyTitle = title.Basic;

    // Animation Purpose
    public float animationDelay = 1;

    public enum title
    {
        Basic,
        Boss
    }
}
