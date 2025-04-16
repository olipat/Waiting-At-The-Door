using UnityEngine;
using System.Collections;

public class DenialPlatform : MonoBehaviour
{
    public SpriteRenderer sr;
    public Sprite one;
    public Sprite two;
    public Sprite three;
    public Sprite four;
    public Sprite five; 

    public float platformLifetime = 3f;
    public float fallSpeed = 0.5f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(HandlePlatformLifecycle());
    }

    IEnumerator HandlePlatformLifecycle()
    {
        //spawning platform 
        sr.sprite = one;
        yield return new WaitForSeconds(0.1f);

        sr.sprite = two;
        yield return new WaitForSeconds(0.1f);

        sr.sprite = three;
        yield return new WaitForSeconds(0.1f);
        sr.sprite = four;
        yield return new WaitForSeconds(0.1f);
        sr.sprite = five;
        yield return new WaitForSeconds(2.1f);

        // get smaller to cue falling
        sr.sprite = four;
        yield return new WaitForSeconds(0.5f);

        // Start Falling
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallSpeed;

        // almost gone
        sr.sprite = three;
        yield return new WaitForSeconds(0.4f);

        sr.sprite = two;
        yield return new WaitForSeconds(0.2f);

        sr.sprite = one;
        yield return new WaitForSeconds(0.2f);

        // Destroy after time limit is up 
        DenialAbility.Instance.RemovePlatform(this.gameObject);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
