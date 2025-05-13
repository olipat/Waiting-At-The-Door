using System.Collections;
using UnityEngine;

public class StopSignEnemy : MonoBehaviour
{
    public Sprite someSprite;
    public float patrolSpeed;
    public float chaseSpeed;
    public float detectionRange;
    public Transform player;

    private Rigidbody2D enemyRB;
    public GameObject groundCheck;
    public GameObject wallCheck;
    public LayerMask groundLayer;

    public bool facingRight;
    public bool isGrounded;
    public bool isWall;

    private bool isBlocking = false;
    private bool isStopping = false;
    private float stopTime = 1f;

    private Animator animator;
    private bool pushedBack = false;
    public float pushDuration = 1.5f;

    private bool playerInTrigger = false;

    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.2f, groundLayer);
        isWall = Physics2D.OverlapCircle(wallCheck.transform.position, 0.2f, groundLayer);

        if (pushedBack || player == null || isStopping) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            isBlocking = true;
        }
        else
        {
            isBlocking = false;
        }

        if (isBlocking)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("shouldStop", false);

        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);

        if ((!isGrounded || isWall))
        {
            Flip();
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("shouldStop", false);

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);

        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isStopping)
        {
            playerInTrigger = true;
            StartCoroutine(StopForDuration());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    private IEnumerator StopForDuration()
    {
        isStopping = true;
        animator.SetBool("shouldStop", true);
        animator.SetBool("isWalking", false);

        yield return new WaitForSeconds(stopTime);

        isStopping = false;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isBlocking = distanceToPlayer < detectionRange;
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        StartCoroutine(StunCoroutine());

        if (force > 0.1f)
        {
            enemyRB.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    private IEnumerator StunCoroutine()
    {
        pushedBack = true;
        enemyRB.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(pushDuration);
        pushedBack = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.green;
        if (groundCheck != null) Gizmos.DrawWireSphere(groundCheck.transform.position, 0.2f);
        if (wallCheck != null) Gizmos.DrawWireSphere(wallCheck.transform.position, 0.2f);
    }
}
