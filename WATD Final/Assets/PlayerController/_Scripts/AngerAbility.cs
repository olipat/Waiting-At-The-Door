using UnityEngine;

public class AngerBarkAbility : MonoBehaviour
{
    public float barkRange = 10f;
    public int damage = 1;
    public float cooldown = 5f; 
    public LayerMask enemyLayer;
    public LayerMask breakableLayer;

    public AudioClip barkclip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void UseAngerBark()
    {
        StartCoroutine(AngerBark());
    }

    private System.Collections.IEnumerator AngerBark()
    {

        if (barkclip != null && audioSource != null)
        {
            audioSource.clip = barkclip;
            audioSource.time = 0.5f;
            audioSource.pitch = 0.7f;
            audioSource.Play();

            float adjustedDuration = 1f / audioSource.pitch;
            yield return new WaitForSeconds(adjustedDuration);

            audioSource.Stop();
            audioSource.pitch = 1f;
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
            Debug.Log("Bark hit: " + obj.name);
            BreakableObject bo = obj.GetComponent<BreakableObject>();
            if (bo != null)
            {
                bo.Break();
                Debug.Log(obj.name + " was broken by Anger Bark");
                continue;
            }
            // Handle stalactites
            Stalactite stalactite = obj.GetComponentInParent<Stalactite>();
            if (stalactite != null)
            {
                stalactite.Shatter();
                Debug.Log(obj.name + " stalactite was shattered by Anger Bark");
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

