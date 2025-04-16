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

    private bool flag = false;

    public SpriteRenderer m_SpriteRenderer;
    public GameObject explosionPreFab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        if(col.tag == "Player" && flag == false)
        {
            playerClose = true;
            StartCoroutine(WaitAndExplode(5f));
            flag = true;
        }
    }

    private IEnumerator WaitAndExplode(float waitTime)
    {
        print("starting wait and explode");
        yield return new WaitForSeconds(4f);
        Explode();
    }

    void Explode()
    {
        m_SpriteRenderer.enabled = false;
        Instantiate(explosionPreFab, this.transform.position, Quaternion.identity);
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);
        foreach (Collider2D hit in hitObjects)
        {
            if (hit.CompareTag("Player"))
            {
                print("bomber did hit the player with their explosion");
                UIcontrolReferemce.GetComponent<UIController>().ApplyDamage();
                AudioManager.instance.PlaySFX(6);
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
