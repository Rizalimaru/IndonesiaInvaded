using System.Collections;
using UnityEngine;

public class DissolveWall : MonoBehaviour
{

    public static DissolveWall instance;
    public float dissolveDuration = 3f; 
    public Material dissolveMaterial;

    public void DissolveWallFunction()
    {
        StartCoroutine(Dissolve());
    }

    public void UnDissolveWallFunction()
    {
        StartCoroutine(UnDissolve());
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
    }



    private IEnumerator Dissolve()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / dissolveDuration;
            dissolveMaterial.SetFloat("_DissolveAmount", t);
            yield return null;
        }

        // menunggu 4 detik kemudian set material dissolve ke 0
        yield return new WaitForSeconds(7f);
        dissolveMaterial.SetFloat("_DissolveAmount", 0);
    }

    private IEnumerator UnDissolve()
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime / dissolveDuration;
            dissolveMaterial.SetFloat("_DissolveAmount", t);
            yield return null;
        }
    }
}
