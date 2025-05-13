using UnityEngine;
public class StopSignEnemy : MonoBehaviour
{
    public Sprite someSprite;
    public float patrolSpeed;
    public float chaseSpeed;
    public float detectionRange;
   // public float stopDistance;
    public Transform player;
    private Rigidbody2D enemyRB;
    public GameObject groundCheck;
    public GameObject wallCheck;
    public LayerMask groundLayer;
    public bool facingRight;
    public bool isGrounded;
    public bool isWall;

    private bool isBlocking;

    Animator animator;
    //adding vars to handle basic bark ability
    private bool pushedBack = false;
    public float pushDuration = 1.5f;

    private bool stop = false;

    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.2f, groundLayer);
        isWall = Physics2D.OverlapCircle(wallCheck.transform.position, 0.2f, groundLayer);

        if (pushedBack || player == null) return; 

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if ((distanceToPlayer < detectionRange) && !isBlocking)
        {
            isBlocking = true;
        }
        if (isBlocking){
            MoveToBlockPlayer();
        }
        else {
            Patrol();
        }
    }

    void Patrol()
    {
        if (isBlocking) return; 

        //enemyRB.linearVelocity = new Vector2((facingRight ? 1 : -1) * patrolSpeed, enemyRB.linearVelocity.y);
        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);

        if ((!isGrounded || isWall) && facingRight)
        {
            Flip();
        }
        else if ((!isGrounded || isWall) && !facingRight)
        {
            Flip();
        }
    }

    void MoveToBlockPlayer()
    {
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        float distance = Mathf.Abs(player.position.x - transform.position.x);


        float stopThreshold = 0.5f;
        float resumeThreshold = 0.7f;

        if (distance > resumeThreshold)
        {
            animator.SetBool("shouldStop", false);
            animator.SetBool("isWalking", true);

            Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
        }
        else if (distance < stopThreshold )
        {
            animator.SetBool("shouldStop", true);
            animator.SetBool("isWalking", false);
        }

        // Face the player
        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.blue;
       // Gizmos.DrawWireSphere(transform.position, stopDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, 0.2f);
        Gizmos.DrawWireSphere(wallCheck.transform.position, 0.2f);
    }

    //functions below added to handle bark ability being applied 
    public void ApplyKnockback(Vector2 direction, float force)
    {
        StartCoroutine(StunCoroutine());

        // now with the reduced bark force it'll stun stop sign guy no matter force
        if (force > 0.1f) 
        {
            enemyRB.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    private System.Collections.IEnumerator StunCoroutine()
    {
        pushedBack = true;
        enemyRB.linearVelocity = Vector2.zero; 

        yield return new WaitForSeconds(1.5f); 

        pushedBack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            stop = true;
            animator.SetBool("shouldStop", true);
            isBlocking = true;
            this.GetComponent<SpriteRenderer>().sprite = someSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isBlocking = false;
            animator.SetBool("shouldStop", false);
        }
    }
}