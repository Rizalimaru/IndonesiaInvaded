using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public Vector3 GetCheckpointPosition()
    {
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.CompareTag("Checkpoint"))
            {
                return checkpoint.transform.position;
            }
        }
        return Vector3.zero;
    }

    public void SetCheckpointPosition(Vector3 position)
    {
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.CompareTag("Checkpoint"))
            {
                checkpoint.transform.position = position;
            }
        }
    }
}
