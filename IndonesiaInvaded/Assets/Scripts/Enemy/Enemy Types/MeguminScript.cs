using System.Collections;
using UnityEngine;

public class MeguminScript : MonoBehaviour
{

    public GameObject mainObjWithCollider;
    public Collider col;
    private Vector3 scaleChanger;
    private float delay;

    private void Awake()
    {
        col = mainObjWithCollider.GetComponent<Collider>();

        delay = 8f;

        Invoke("EnableCollider", 8.5f);

        StartCoroutine(Scaler());

        GameObject.Destroy(gameObject, 12f);

        scaleChanger = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void EnableCollider()
    {
        col.enabled = true;
    }

    private IEnumerator Scaler()
    {
        yield return new WaitForSeconds(8);

        float counter = 0;

        Vector3 startSize = mainObjWithCollider.transform.localScale;

        while (counter < 1.5)
        {
            counter += Time.deltaTime;
            mainObjWithCollider.transform.localScale = Vector3.Lerp(startSize, new Vector3(0.1f, mainObjWithCollider.transform.localScale.y, 0.1f), counter / 1.5f);
            yield return null;
        }
    }
}
