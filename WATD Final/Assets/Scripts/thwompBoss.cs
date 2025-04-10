using UnityEngine;
using System.Collections;

public class thwompBoss : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float fallSpeed = 10f;
    public float resetDelay = 2f;

    private Vector3 originalPosition;
    private bool isFalling = false;
    private Rigidbody2D rb;

    public GameObject UIcontrolReferemce;

    void Start()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;  // Start as kinematic to prevent falling
        UIcontrolReferemce = GameObject.FindGameObjectWithTag("UiControl");
    }

    public void fall()
    {
        StartCoroutine(ShakeAndFall());
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
        if (collision.gameObject.layer == 6)
        {
            StartCoroutine(ResetPosition());
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            print("thwomp hit player");
            UIcontrolReferemce.GetComponent<UIController>().ApplyDamage();
        }
    }

    IEnumerator ResetPosition()
    {
        print("ground");
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;
        yield return new WaitForSeconds(resetDelay);

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
