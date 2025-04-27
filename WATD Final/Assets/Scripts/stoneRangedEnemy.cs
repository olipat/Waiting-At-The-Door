using UnityEngine;
using System.Collections;

public class stoneRangedEnemy : stoneEnemy
{
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            comeAlive();
        }
    }

    void comeAlive()
    {
        animator.SetTrigger("awake");
        StartCoroutine(waitBeforeAttack());
    }

    IEnumerator waitBeforeAttack()
    {
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("attack");
    }
}
