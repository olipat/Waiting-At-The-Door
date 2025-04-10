using UnityEngine;
using System.Collections;

public class spike : MonoBehaviour
{
    public GameObject bigSpike;
    public SpriteRenderer m_SpriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void attack()
    {
       // print("spike attack called");
        m_SpriteRenderer.enabled = true;
        StartCoroutine(spikeAttack());
    }

    IEnumerator spikeAttack()
    {
        yield return new WaitForSeconds(1f);

        bigSpike.SetActive(true);
        m_SpriteRenderer.enabled = false;
        yield return new WaitForSeconds(1.5f);

        bigSpike.SetActive(false);
    }
}
