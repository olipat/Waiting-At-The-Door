using UnityEngine;
using System.Collections;

public class Thwomp : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float fallSpeed = 10f;
    public float resetDelay = 2f;

    private Vector3 originalPosition;
    private bool isFalling = false;
    private Rigidbody2D rb;

    public GameObject UIcontrolReferemce;

    public SpriteRenderer m_SpriteRenderer;
    public GameObject explosionPreFab;

    Animator animator;

    bool groundCollisionExplosionFlag = false;
    void Start()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;  // Start as kinematic to prevent falling
        UIcontrolReferemce = GameObject.FindGameObjectWithTag("UiControl");
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFalling && other.CompareTag("Player"))
        {
            animator.SetTrigger("sleep");
            StartCoroutine(ShakeAndFall());
        }
    }

    IEnumerator ShakeAndFall()
    {
        isFalling = true;

        // Shake effect
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            transform.position = originalPosition + (Vector3)Random.insideUnitCircle * 0.1f;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        rb.isKinematic = false;
        rb.linearVelocity = new Vector2(0, -fallSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && groundCollisionExplosionFlag == false)
        {
            groundCollisionExplosionFlag = true;
            m_SpriteRenderer.enabled = false;
            Instantiate(explosionPreFab, this.transform.position, Quaternion.identity);
            StartCoroutine(ResetPosition());
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            print("thwomp hit player");
            UIcontrolReferemce.GetComponent<UIController>().ApplyDamage();
            AudioManager.instance.PlaySFX(6);
        }
    }

    IEnumerator ResetPosition()
    {
        groundCollisionExplosionFlag = false;
        print("ground");
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        yield return new WaitForSeconds(resetDelay);

        m_SpriteRenderer.enabled = true;

        float elapsed = 0f;
        Vector3 startPos = transform.position;
        while (elapsed < 1f)
        {
            //transform.Move Vector3.Lerp(startPos, originalPosition, elapsed);
            rb.MovePosition(Vector3.Lerp(startPos, originalPosition, elapsed));
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(originalPosition);
        isFalling = false;
        
    }
}
