using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance;
    private List<Vector3> checkpointPositions = new List<Vector3>();
    private void Awake()
    {
        instance = this;
    }

    public void AddCheckpoint(Vector3 position)
    {
        checkpointPositions.Add(position);
    }
    public Vector3 GetLastCheckpointPosition()
    {
        if (checkpointPositions.Count > 0)
        {
            return checkpointPositions[checkpointPositions.Count - 1];
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void ClearCheckpoints()
    {
        checkpointPositions.Clear();
    }

    public void Respawn()
    {
        if (checkpointPositions.Count > 0)
        {
            Vector3 lastCheckpointPosition = GetLastCheckpointPosition();
            PlayerDataSaving.instance.Teleports(lastCheckpointPosition);
        }
        else
        {
            Debug.LogError("No checkpoints found!");
        }
    }
}
