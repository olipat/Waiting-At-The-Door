using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float fallDelay = 1.5f;
    public bool hasFallen = false;
    private SpriteRenderer[] spriteRenderers;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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
        rb.gravityScale = 1f; 
    }

    //Use this later for sniff ability 
    public void WarnPlatform(float warningDuration)
    {
        StartCoroutine(GlowRedTemporarily(warningDuration));
    }

    private IEnumerator GlowRedTemporarily(float duration)
    {
        Color originalColor = spriteRenderers[0].color;

        foreach (SpriteRenderer sr in spriteRenderers)
            sr.color = Color.red;

        yield return new WaitForSeconds(duration);

        foreach (SpriteRenderer sr in spriteRenderers)
            sr.color = originalColor;
    }

    public void ResetPlatform(Vector3 resetPosition)
    {
        StopAllCoroutines();

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        hasFallen = false;
        transform.rotation = Quaternion.identity;
        transform.position = resetPosition;
    }
}
