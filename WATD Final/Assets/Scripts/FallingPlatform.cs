using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float fallDelay = 1.5f;
    private bool hasFallen = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasFallen && collision.collider.CompareTag("Player"))
        {
            StartCoroutine(FallAfterDelay());
        }
    }

    private System.Collections.IEnumerator FallAfterDelay()
    {
        hasFallen = true;
        yield return new WaitForSeconds(fallDelay);

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f; // Ensure it falls
    }

    // Called by sniff ability to warn player
    public void WarnPlatform(float warningDuration)
    {
        StartCoroutine(GlowRedTemporarily(warningDuration));
    }

    private System.Collections.IEnumerator GlowRedTemporarily(float duration)
    {
        if (sr == null) yield break;

        Color originalColor = sr.color;
        sr.color = Color.red;

        yield return new WaitForSeconds(duration);

        sr.color = originalColor;
    }
}
