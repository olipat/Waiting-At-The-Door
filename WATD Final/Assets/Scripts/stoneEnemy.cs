using UnityEngine;
using System.Collections;

public class stoneEnemy : MonoBehaviour
{
    Animator animator;
    private bool angry = false;
    public Transform player;
    public float chaseSpeed;
    public bool facingRight;
    //Adding this variable so that regular bark stuns enemy
    private bool stunned = false;
    public Sprite sleeping;
    private Sprite og;
    private SpriteRenderer sr;
    public GameObject UIcontrolReferemce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if (sr != null){
            og = sr.sprite;
        }
        UIcontrolReferemce = GameObject.FindGameObjectWithTag("UiControl");
    }

    // Update is called once per frame
    void Update()
    {
        if (angry && !stunned)
        {
            float direction = player.position.x > transform.position.x ? 1 : -1;

            //enemyRB.linearVelocity = new Vector2(direction * chaseSpeed, enemyRB.linearVelocity.y);

            float Direction = Mathf.Sign(player.position.x - transform.position.x);
            Vector2 MovePos = new Vector2(
                transform.position.x + Direction * chaseSpeed,
                transform.position.y
            );
            transform.position = MovePos;

            // Ensure the enemy faces the player
            if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
            {
                Flip();
            }
        }
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
    {;
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("attack");
        angry = true;
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && angry && !stunned)
        {
            AudioManager.instance.PlaySFX(6);
            UIcontrolReferemce.GetComponent<UIController>().ApplyDamage();
        }
    }
    //shatter bark used, break enemy
    public void HandleShatterBark()
    {
        if (!angry) return; 

        StartCoroutine(BreakCoroutine());
    }
    //regular bark used, stun enemy
    public void HandleNormalBark()
    {
        if (!angry && stunned) return;

        StartCoroutine(StunCoroutine(3f));
    }
    
    //stun the enemy for 1 second
    private IEnumerator StunCoroutine(float duration)
    {
        stunned = true;
        //can add animation here if we want later
        yield return new WaitForSeconds(duration);
        stunned = false;
    }

    //break the enemy for 3 seconds, then rebuild again and attack
    private IEnumerator BreakCoroutine()
    {
        stunned = true;

        if (animator != null)
            animator.enabled = false; 

        if (sr != null && sleeping != null)
            sr.sprite = sleeping; 

        yield return new WaitForSeconds(3f); 

        if (animator != null)
            animator.enabled = true; 

        sr.sprite = og; 

        animator.SetTrigger("awake"); 
        yield return new WaitForSeconds(1f); 
        animator.SetTrigger("attack");

        stunned = false;
    }

}
