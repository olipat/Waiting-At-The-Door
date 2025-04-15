using UnityEngine;

public class BarkAbility : MonoBehaviour
{
    public float barkRange = 10f;
    public float pushForce = 20f;
    public KeyCode barkKey = KeyCode.E;
    public LayerMask enemyLayer;
    public AudioClip barkClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(barkKey) || Input.GetButtonDown("Fire3"))
        {
            Bark();
        }
    }

    void Bark()
    {
         if (barkClip != null && audioSource != null)
        {
            StartCoroutine(PlayBarkForOneSecond());
        }
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
    System.Collections.IEnumerator PlayBarkForOneSecond()
    {
        audioSource.clip = barkClip;
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        audioSource.Stop();
    }
}
