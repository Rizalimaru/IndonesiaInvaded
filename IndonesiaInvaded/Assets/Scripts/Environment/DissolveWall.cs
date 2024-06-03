


using System.Collections;
using UnityEngine;

public class DissolveWall : MonoBehaviour
{
    public static DissolveWall instance;
    public float dissolveDuration = 3f;
    public Material[] dissolveMaterials;


    private void Awake()
    {
        instance = this;
    }

    public void DissolveWallFunction()
    {
        StartCoroutine(Dissolve());
    }

    public void UnDissolveWallFunction()
    {

        StartCoroutine(UnDissolve());
    }

    private IEnumerator Dissolve()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / dissolveDuration;
            SetDissolveAmount(t);
            yield return null;
        }

        // Wait for 4 seconds then reset the dissolve material to 0
        yield return new WaitForSeconds(5f);
        SetDissolveAmount(0);
    }

    private IEnumerator UnDissolve()
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime / dissolveDuration;
            SetDissolveAmount(t);
            yield return null;
        }

    }

    private void SetDissolveAmount(float amount)
    {
        foreach (var material in dissolveMaterials)
        {
            material.SetFloat("_DissolveAmount", amount);
        }
    }
}
