using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager instance { get; private set; }
    [SerializeField] Animator animator;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject player;

    [SerializeField] Vector3 checkpointPosition;

    private void Awake()
    {
        instance = this;
    }

    public void SetCheckPoint(Vector3 position)
    {
        // vectorPoint = position;
        checkpointPosition = new Vector3(position.x, 0, position.z);
    }

    public void Respawn()
    {
        StartCoroutine(RespawnFade());

    }

    public Vector3 GetCheckPoint()
    {
        // return vectorPoint;
        return checkpointPosition;
    }

    public void ResetCheckPoint()
    {
        // vectorPoint = Vector3.zero;
        checkpointPosition = new Vector3(0, 0, 0);
    }

    IEnumerator RespawnFade()
    {
        animator.SetTrigger("End");
        yield return new WaitForSeconds(1);
        if (PlayerMovement.instance.canMove == false)
        {
            PlayerMovement.instance.canMove = true;
        }
        PlayerMovement.instance.canMove = true;
        PlayerAttribut.instance.ResetTotal();
        ScoreManager.instance.ResetScore();
        // UI_PauseGame.instance.player.SetActive(true);
        PlayerDataSaving.instance.Teleport(GetCheckPoint(), Quaternion.identity);
        animator.SetTrigger("Start");
    }
}
