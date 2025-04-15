using UnityEngine;
using System;

public class Stalactite : MonoBehaviour
{
    public Sprite brokenTopSprite;
    public Sprite bottomBrokenSprite;
    public GameObject topPiece;
    public GameObject bottomPiece;

    private SpriteRenderer topRenderer;
    private Rigidbody2D bottomRb;
    private bool hasFallen = false;
    public event Action OnBroken;

    private Coroutine sinkRoutine;


    private void Start()
    {
        topRenderer = topPiece.GetComponent<SpriteRenderer>();
        bottomRb = bottomPiece.GetComponent<Rigidbody2D>();
        bottomRb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Shatter()
    {
        if (hasFallen) return;
        hasFallen = true;

        if (OnBroken != null)
        OnBroken.Invoke();

        // Change top piece to broken sprite
        if (brokenTopSprite != null && bottomBrokenSprite != null)
            topRenderer.sprite = brokenTopSprite;
            bottomPiece.GetComponent<SpriteRenderer>().sprite = bottomBrokenSprite;

        // Make the bottom fall
        bottomRb.bodyType = RigidbodyType2D.Dynamic;
        bottomRb.gravityScale = 1.5f;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasFallen) return;

        bottomRb.linearVelocity = Vector2.zero;
        bottomRb.angularVelocity = 0f;

        // Lava tiles on the lava layer 
        if (collision.gameObject.CompareTag("Lava"))
        {
            Debug.Log("Stalactite hit lava!");
            sinkRoutine = StartCoroutine(SinkIntoLava());
        }
        else
        {
            bottomRb.bodyType = RigidbodyType2D.Static;
            bottomRb.constraints = RigidbodyConstraints2D.FreezeAll;
            Debug.Log("Stalactite hit ground.");
        }
    }

    private System.Collections.IEnumerator SinkIntoLava()
    {
        float sinkSpeed = 0.5f;
        float sinkDuration = 7.5f;
        float elapsed = 0f;

        while (elapsed < sinkDuration)
        {
            bottomPiece.transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        bottomPiece.SetActive(false);
    }

    public void ResetStalactite(Vector3 topPos, Vector3 bottomPos, Sprite originalTopSprite, Sprite originalBottomSprite)
    {
        if (sinkRoutine != null)
        {
            StopCoroutine(sinkRoutine);
            sinkRoutine = null;
        }
        hasFallen = false;

        topPiece.transform.position = topPos;
        bottomPiece.transform.position = bottomPos;

        topRenderer.sprite = originalTopSprite;
        bottomPiece.GetComponent<SpriteRenderer>().sprite = originalBottomSprite;

        bottomRb.linearVelocity = Vector2.zero;
        bottomRb.angularVelocity = 0f;
        bottomRb.bodyType = RigidbodyType2D.Kinematic;
        bottomRb.gravityScale = 0f;
        bottomRb.constraints = RigidbodyConstraints2D.None;

        bottomPiece.SetActive(true);
    }

}
