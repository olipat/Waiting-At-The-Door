using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage = 1f;
    public float lifetime = 5f;

    private void Start()
    {
        Debug.Log("Projectile spawned: " + gameObject.name);
        Destroy(gameObject, lifetime); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Projectile hit: " + collision.name);
        if (collision.CompareTag("Player"))
        {
            if (UIController.Instance != null)
            {
                UIController.Instance.ApplyDamage((int)damage);
            }

            Destroy(gameObject);
        }
        else if (!collision.isTrigger && !collision.CompareTag("Enemy") && !collision.CompareTag("PhantomPlat"))
        {
            Destroy(gameObject);
        }
    }
}
