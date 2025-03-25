using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;
    public int damage = 10;

    private Vector3 targetDirection;

    void Start()
    {
        targetDirection = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += targetDirection * speed * Time.deltaTime;
    }

    //void OnTriggerEnter2D(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        print("tire hit player");
    //        Destroy(gameObject);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("tire hit player");
            Destroy(gameObject);
        }
    }

}
