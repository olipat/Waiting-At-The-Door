using UnityEngine;

public class BarkAbility : MonoBehaviour
{
    public float barkRange = 10f;
    public float pushForce = 20f;
    public KeyCode barkKey = KeyCode.E;
    public LayerMask enemyLayer;

    void Update()
    {
        if (Input.GetKeyDown(barkKey))
        {
            Bark();
        }
    }

    void Bark()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, barkRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Small Enemy"))
            {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                Vector2 pushDir = (hit.transform.position - transform.position).normalized;

                StopSignEnemy sse = hit.GetComponent<StopSignEnemy>();
                if (sse != null)
                {
                    sse.ApplyKnockback(pushDir, pushForce);
                }
                else if (rb != null)
                {
                    rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
                }
                
            }
        }
    }

}
