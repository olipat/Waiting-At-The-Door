using UnityEngine;
public class StopSignEnemy : MonoBehaviour
{
    public float patrolSpeed;
    public float chaseSpeed;
    public float detectionRange;
    public float stopDistance;
    public Transform player;
    private Rigidbody2D enemyRB;
    public GameObject groundCheck;
    public GameObject wallCheck;
    public LayerMask groundLayer;
    public bool facingRight;
    public bool isGrounded;
    public bool isWall;

    private bool isBlocking;
    //adding vars to handle basic bark ability
    private bool pushedBack = false;
    public float pushDuration = 1f;

    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.2f, groundLayer);
        isWall = Physics2D.OverlapCircle(wallCheck.transform.position, 0.2f, groundLayer);

        if (pushedBack || player == null) return; // Avoid errors if no player is assigned

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            MoveToBlockPlayer(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (isBlocking) return; // Prevent patrolling while blocking

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

    void MoveToBlockPlayer(float distanceToPlayer)
    {
        isBlocking = true;

        float direction = player.position.x > transform.position.x ? 1 : -1;

        // Move towards the player at a faster speed
        if (distanceToPlayer > stopDistance)
        {
            enemyRB.linearVelocity = new Vector2(direction * chaseSpeed, enemyRB.linearVelocity.y);
        }
        else
        {
            //enemyRB.linearVelocity = Vector2.zero; // Stop in front of player
        }

        // Ensure the enemy faces the player
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
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, 0.2f);
        Gizmos.DrawWireSphere(wallCheck.transform.position, 0.2f);
    }

    //functions below added to handle bark ability being applied 
    public void ApplyKnockback(Vector2 direction, float force)
    {
        enemyRB.AddForce(direction * force, ForceMode2D.Impulse);
        StartCoroutine(KnockbackCoroutine());
    }

    private System.Collections.IEnumerator KnockbackCoroutine()
    {
        pushedBack = true;
        enemyRB.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(pushDuration);

        pushedBack = false;
    }
}