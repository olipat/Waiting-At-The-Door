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
            Destroy(gameObject);
        }
    }
}
