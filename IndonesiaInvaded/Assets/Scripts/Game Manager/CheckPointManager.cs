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

        DeactiveObject();

        yield return new WaitForSeconds(1);

        ScoreManager.instance.ResetScore();
        PlayerDataSaving.instance.Teleport(GetCheckPoint(), Quaternion.identity);

        ActiveObject();

        animator.SetTrigger("Start");
    }

    private void ActiveObject()
    {
        player.gameObject.SetActive(true);
        playerCamera.SetActive(true);
    }

    private void DeactiveObject()
    {
        player.gameObject.SetActive(false);
        playerCamera.SetActive(false);
    }

}
