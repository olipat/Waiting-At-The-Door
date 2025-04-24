using UnityEngine;

public class DestructibleByBoulder : MonoBehaviour
{
    public Sprite builtSprite;
    public Sprite brokenSprite;

    private SpriteRenderer sr;
    private Collider2D col;
    private bool isBroken = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (sr != null && builtSprite != null)
            sr.sprite = builtSprite;
        
        if (col != null)
            col.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBroken) return;

        if (collision.collider.CompareTag("Boulder"))
        {
            BreakObject();
            collision.collider.gameObject.SetActive(false);
        }
    }

    void BreakObject()
    {
        isBroken = true;

        if (sr != null && brokenSprite != null)
            sr.sprite = brokenSprite;

        if (col != null)
            col.enabled = false;

        Debug.Log(gameObject.name + " was broken by a boulder.");
    }
}

