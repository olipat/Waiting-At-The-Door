using UnityEngine;
using System.Collections;

public class boomer : MonoBehaviour
{
    public float speed;
    public float chaseSpeed;
    public float radius;
    private Rigidbody2D EnemyRB;
    public LayerMask groundLayer;
    public bool facingRight;

    public GameObject groundCheck;
    public GameObject wallCheck;

    public Transform player;

    public bool playerClose = false;

    public float explosionRadius = 2f;
    public float explosionDelay = 2f;
    public LayerMask playerLayer;
    public bool isWall;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public GameObject UIcontrolReferemce;

    public bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIcontrolReferemce = GameObject.FindGameObjectWithTag("UiControl");
        EnemyRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        if(playerClose == false)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, radius, groundLayer);
            isWall = Physics2D.OverlapCircle(wallCheck.transform.position, radius, groundLayer);
            if ((!isGrounded || isWall) && facingRight)
            {
                Flip();
            }
            else if ((!isGrounded || isWall) && !facingRight)
            {
                Flip();
            }
        }
        else
        {
            float direction = player.position.x > transform.position.x ? 1 : -1;

            float Direction = Mathf.Sign(player.position.x - transform.position.x);
            Vector2 MovePos = new Vector2(
                transform.position.x + Direction * chaseSpeed,
                transform.position.y
            );
            transform.position = MovePos;

            // Ensure the enemy faces the player
            if ((direction < 0 && !facingRight) || (direction > 0 && facingRight))
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            playerClose = true;
        }
        StartCoroutine(WaitAndExplode(5f));
    }

    private IEnumerator WaitAndExplode(float waitTime)
    {
        print("starting wait and explode");
        float flashSpeed = 0.2f;
        for (float t = 0; t < waitTime; t += flashSpeed)
        {
            spriteRenderer.color = (spriteRenderer.color == originalColor) ? Color.red : originalColor;
            yield return new WaitForSeconds(flashSpeed);
        }

        spriteRenderer.color = originalColor;
        Explode();
    }

    void Explode()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);
        foreach (Collider2D hit in hitObjects)
        {
            if (hit.CompareTag("Player"))
            {
                print("hit player");
                UIcontrolReferemce.GetComponent<UIController>().ApplyDamage();
            }
        }
        print("should destroy");

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, radius);
        Gizmos.DrawWireSphere(wallCheck.transform.position, radius);
    }

}
