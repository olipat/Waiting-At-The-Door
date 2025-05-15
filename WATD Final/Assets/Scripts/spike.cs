using UnityEngine;
using System.Collections;

public class spike : MonoBehaviour
{
    public GameObject bigSpike;
    public SpriteRenderer m_SpriteRenderer;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    animator.SetTrigger("play");
        //}
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
        print("should play spike anim");
        animator.SetTrigger("play");
        bigSpike.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        bigSpike.SetActive(false);
        m_SpriteRenderer.enabled = false;
    }
}
