using System;
using System.Linq;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
//     // List of checkpoints
//     public Checkpoint[] checkpoints;
//     private GameData gameData;

//     // Player's transform
//     public Transform playerTransform;

//     void Start()
//     {
//         // Initialize checkpoints
//         foreach (Checkpoint checkpoint in checkpoints)
//         {
//             checkpoint.Initialize();
//         }
//     }

//     void Update()
//     {
//         // Check if player has reached a checkpoint
//         foreach (Checkpoint checkpoint in checkpoints)
//         {
//             if (checkpoint.IsReached(playerTransform.position))
//             {
//                 // Save current checkpoint index
//                 gameData.currentCheckpointIndex = Array.IndexOf(checkpoints, checkpoint);

//                 // Load checkpoint data
//                 LoadCheckpointData(checkpoint);

//                 break;
//             }
//         }
//     }

//     void LoadCheckpointData(Checkpoint checkpoint)
//     {
//         // Load player's position and rotation
//         playerTransform.position = checkpoint.position;
//         playerTransform.rotation = checkpoint.rotation;

//         // Load other game state data (e.g. health, score, etc.)
//         //...
//     }

//     public void CreateCheckpoint()
//     {
//         // Create a new checkpoint
//         Checkpoint newCheckpoint = new Checkpoint();

//         // Set checkpoint position and rotation
//         newCheckpoint.position = playerTransform.position;
//         newCheckpoint.rotation = playerTransform.rotation;

//         // Add new checkpoint to the list
//         checkpoints = checkpoints.Concat(new Checkpoint[] { newCheckpoint }).ToArray();

//         // Update current checkpoint index
//         gameData.currentCheckpointIndex = checkpoints.Length - 1;
//     }

//     public void LoadLastCheckpoint()
//     {
//         // Load the last checkpoint data
//         LoadCheckpointData(checkpoints[gameData.currentCheckpointIndex]);
//     }
}
