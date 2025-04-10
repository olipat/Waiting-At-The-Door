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
    private Vector3 home;
    public float rightDist;
    public float leftDist;
    public Transform player;

    public bool playerClose = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyRB = GetComponent<Rigidbody2D>();
        home = transform.position;
    }

    void FixedUpdate()
    {

        if(playerClose == false)
        {
            if (facingRight)
            {
                EnemyRB.MovePosition(transform.position + Vector3.right * speed * Time.deltaTime);
                if ((transform.position.x - home.x) > rightDist)
                {
                    Flip();
                }
            }
            else
            {
                EnemyRB.MovePosition(transform.position + Vector3.left * speed * Time.deltaTime);
                if ((home.x - transform.position.x) > leftDist)
                {
                    Flip();
                }
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
            if ((direction > 0 && !facingRight) || (direction < 0 && facingRight))
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * rightDist));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.left * leftDist));
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            playerClose = true;
        }
        StartCoroutine(WaitAndExplode(10f));
    }

    private IEnumerator WaitAndExplode(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("enemy exploded!");

    }

}
