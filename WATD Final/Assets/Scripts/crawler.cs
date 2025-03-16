using UnityEngine;

public class crawler : MonoBehaviour
{

    public float speed;
    public float radius;
    private Rigidbody2D EnemyRB;
    public GameObject groundCheck;
    public LayerMask groundLayer;
    public bool facingRight;
    public bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //EnemyRB.linearVelocity = Vector2.right * speed * Time.deltaTime;
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, radius, groundLayer);
        if (!isGrounded && facingRight)
        {
            Flip();
        }
        else if (!isGrounded && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        //speed = -speed;
        print("FLIP");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, radius);
    }
}
