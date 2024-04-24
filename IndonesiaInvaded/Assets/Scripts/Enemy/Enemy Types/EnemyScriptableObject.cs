using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    // Attribute Config
    public float Health = 100;
    public float attackPower = 10;
    public float attackSpeed = 1.0f;
    public float triggerDistance = 5;
    public float attackDistance = 1;
    public float attackForce = 1000;
    public float attackDecay = 0.1f; // Dictate the life of attack collider

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

}
