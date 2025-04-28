using DG.Tweening;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public bool dropsWhenBroken = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Break()
    {
        if (dropsWhenBroken && rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
        }
        else
        {
            // Shrink and fade out before destroying
            Sequence breakSequence = DOTween.Sequence();

            breakSequence.Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                breakSequence.Join(sr.DOFade(0f, 0.3f));
            }
            //breakSequence.Join(GetComponent<SpriteRenderer>().DOFade(0f, 0.3f));
            breakSequence.OnComplete(() => Destroy(gameObject));
        }
    }
}
