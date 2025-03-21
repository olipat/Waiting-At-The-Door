using UnityEngine;

public class AngerBarkAbility : MonoBehaviour
{
    public float barkRange = 10f;
    public int damage = 1;
    public float cooldown = 5f; 
    public LayerMask enemyLayer;
    public LayerMask breakableLayer;

    //Add sound here later 
    public AudioSource barkSound;

    public void UseAngerBark()
    {
        StartCoroutine(AngerBark());
    }

    private System.Collections.IEnumerator AngerBark()
    {
        Debug.Log("Anger Bark activated!");

        // sound for later
        if (barkSound != null)
        {
            barkSound.Play();
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, barkRange, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Small Enemy"))
            {
                EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
                if (eh != null)
                {
                    eh.TakeDamage(damage);
                    Debug.Log(enemy.name + " took " + damage + " damage from Anger Bark");
                }
            }
        }

        Collider2D[] breakables = Physics2D.OverlapCircleAll(transform.position, barkRange, breakableLayer);
        foreach (Collider2D obj in breakables)
        {
            BreakableObject bo = obj.GetComponent<BreakableObject>();
            if (bo != null)
            {
                bo.Break();
                Debug.Log(obj.name + " was broken by Anger Bark");
            }
        }

        yield return null; 
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, barkRange);
    }
}

