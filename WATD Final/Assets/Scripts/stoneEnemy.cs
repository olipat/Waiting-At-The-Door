using UnityEngine;
using System.Collections;

public class stoneEnemy : MonoBehaviour
{
    Animator animator;
    public enum EnemyType{chase, shoot}
    public EnemyType enemyType = EnemyType.chase;
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

    //projectile stuff
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootCooldown = 2f;
    private float shootTimer = 0f;
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
            switch (enemyType)
            {
                case EnemyType.chase:
                    HandleChaseBehavior();
                    break;
                case EnemyType.shoot:
                    HandleShootBehavior();
                    break;
            }
        }
    }

    void HandleChaseBehavior()
    {
        //if (!angry || stunned) return;

        float direction = player.position.x > transform.position.x ? 1 : -1;
        float Direction = Mathf.Sign(player.position.x - transform.position.x);
        Vector2 MovePos = new Vector2(
            transform.position.x + Direction * chaseSpeed,
            transform.position.y
        );
        transform.position = MovePos;

        animator.SetBool("isWalking", true);

        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {
            Flip();
        }
    }

    void HandleShootBehavior()
    {
        float direction = player.position.x > transform.position.x ? 1 : -1;
        float Direction = Mathf.Sign(player.position.x - transform.position.x);
        Vector2 MovePos = new Vector2(
            transform.position.x + Direction * chaseSpeed,
            transform.position.y
        );
        transform.position = MovePos;

        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {
            Flip();
        }

        if (Mathf.Abs(Direction) > 0.01f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            animator.SetTrigger("attack 0");
            ShootProjectile();
            shootTimer = shootCooldown;
        }
    }


    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Vector2 dir = (player.position - firePoint.position).normalized;
            proj.GetComponent<Rigidbody2D>().linearVelocity = dir * 6f; 
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !angry)
        {
            animator.SetTrigger("WakeUp 0");
            StartCoroutine(waitBeforeAttack());
        }
    }

    IEnumerator waitBeforeAttack()
    {;
        yield return new WaitForSeconds(1f);
        animator.SetBool("isWalking", true);
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
        angry = false;

        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true); 

        if (sr != null && sleeping != null)
            sr.sprite = sleeping;

        yield return new WaitForSeconds(3f);

        sr.sprite = og;

        animator.SetBool("isIdle", false);  
        animator.SetTrigger("WakeUp 0"); 
        yield return new WaitForSeconds(1f);

        animator.SetBool("isWalking", true); 
        angry = true;
        stunned = false;
    }

}
