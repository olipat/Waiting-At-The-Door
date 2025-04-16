using UnityEngine;
using System.Collections;

public class waitAndDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaitAndExplode(1.5f));
    }

    private IEnumerator WaitAndExplode(float waitTime)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
