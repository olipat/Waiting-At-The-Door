using UnityEngine;

public class SmokePuff : MonoBehaviour
{
    public float floatSpeed = 0.5f;
    public float fadeDuration = 1f;

    private SpriteRenderer sr;
    private float timer = 0f;
    private Color originalColor;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;
        }
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        if (sr != null)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }

        if (timer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}

