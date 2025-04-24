using UnityEngine;
using System.Collections;

public class stoneEnemy : MonoBehaviour
{
    Animator animator;
    private bool angry = false;
    public Transform player;
    public float chaseSpeed;
    public bool facingRight;
    public GameObject UIcontrolReferemce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        UIcontrolReferemce = GameObject.FindGameObjectWithTag("UiControl");
    }

    // Update is called once per frame
    void Update()
    {
        if (angry)
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
        if (col.gameObject.tag == "Player")
        {

            AudioManager.instance.PlaySFX(6);
            UIcontrolReferemce.GetComponent<UIController>().ApplyDamage();
        }
    }
}
