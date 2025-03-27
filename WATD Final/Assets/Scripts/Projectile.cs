using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 3f;
    public int damageAmount = 20;
    private Rigidbody2D rb;
    public int direction = -1; // 1 = right, -1 = left
    private GameObject UIcontrolReferemce;
    public float maxLifetime = 10f; // Maximum time before despawning

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UIcontrolReferemce = GameObject.FindGameObjectWithTag("UiControl");
        UIController.Instance.tireList.Add(this);
        Destroy(gameObject, maxLifetime);
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Wall"))
        //{
        //    direction *= -1;//everse direction when hitting a wall
        //}
        if (collision.gameObject.CompareTag("Player"))
        {
            UIcontrolReferemce.GetComponent<UIController>().ApplyDamage();
            AudioManager.instance.PlaySFX(6);
            Destroy(gameObject);//disappears after hitting player
        }
    }
}
