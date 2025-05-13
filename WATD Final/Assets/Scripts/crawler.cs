using UnityEngine;
using System.Collections;
public class crawler : MonoBehaviour
{

    public float speed;
    public float radius;
    private Rigidbody2D EnemyRB;
    public GameObject groundCheck;
    public GameObject wallCheck;
    public LayerMask groundLayer;
    public bool facingRight;
    public bool isGrounded;
    public bool isWall;
    private bool frozen = false;
    public GameObject UIcontrolReferemce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyRB = GetComponent<Rigidbody2D>();
        UIcontrolReferemce = GameObject.FindGameObjectWithTag("UiControl");
    }

    // Update is called once per frame
    void Update()
    {
        //EnemyRB.linearVelocity = Vector2.right * speed * Time.deltaTime;
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, radius, groundLayer);
        isWall = Physics2D.OverlapCircle(wallCheck.transform.position, radius, groundLayer);
        if ((!isGrounded || isWall) && facingRight && !frozen)
        {
            Flip();
        }
        else if ((!isGrounded || isWall) && !facingRight && !frozen)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        //speed = -speed;
        //print("FLIP");
        frozen = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && !frozen)
        {
            frozen = true;

            AudioManager.instance.PlaySFX(6);
            UIcontrolReferemce.GetComponent<UIController>().ApplyDamage();
            StartCoroutine(Freeze());
        }
        if (col.gameObject.CompareTag("Wall")){
            Flip();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, radius);
        Gizmos.DrawWireSphere(wallCheck.transform.position, radius);
    }

    IEnumerator Freeze()
    {
        frozen = true;
        yield return new WaitForSeconds(1f);
        frozen = false;
    }
}
