using UnityEngine;

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
        StartCoroutine(spikeAttack());
    }

    IEnumerator spikeAttack()
    {
        yield return new WaitForSeconds(1f);

        bigSpike.SetActive(true);
        m_SpriteRenderer.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        bigSpike.SetActive(false);
        m_SpriteRenderer.SetActive(true);
    }
}
