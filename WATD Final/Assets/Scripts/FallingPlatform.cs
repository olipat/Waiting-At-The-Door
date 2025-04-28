using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float fallDelay = 1.5f;
    public float fallDuration = 3f;
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
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y <= -0.5f) 
                {
                    StartCoroutine(FallAfterDelay());
                    break;
                }
            }
        }
    }

    private System.Collections.IEnumerator FallAfterDelay()
    {
        hasFallen = true;
        transform.DOShakePosition(0.5f, new Vector3(0.2f, 0.2f, 0), 10, 90, false, true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(fallDelay-0.5f);

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f; 

        yield return new WaitForSeconds(fallDuration);
        gameObject.SetActive(false);
    }

    ////Use this later for sniff ability 
    //public void WarnPlatform(float warningDuration)
    //{
    //    StartCoroutine(GlowRedTemporarily(warningDuration));
    //}


    //private IEnumerator GlowRedTemporarily(float duration)
    //{
    //    Color originalColor = spriteRenderers[0].color;

    //    foreach (SpriteRenderer sr in spriteRenderers)
    //        sr.color = Color.red;

    //    yield return new WaitForSeconds(duration);

    //    foreach (SpriteRenderer sr in spriteRenderers)
    //        sr.color = originalColor;
    //}

    public void WarnPlatform(float warningDuration)
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr == null) continue;

            Color originalColor = sr.color;

            DG.Tweening.Sequence warnSequence = DOTween.Sequence();

            warnSequence.Append(sr.DOColor(Color.red, 0.2f)) // Fade to red
                        .AppendInterval(warningDuration)      // Stay red
                        .Append(sr.DOColor(originalColor, 0.2f)); // Fade back to original
        }
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
