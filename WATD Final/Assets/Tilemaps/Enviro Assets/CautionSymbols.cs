using UnityEngine;

public class FadeAndDestroy : MonoBehaviour
{
    public float fadeDuration = 1f;
    private SpriteRenderer sr;
    private float timer = 0f;
    private float startAlpha = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startAlpha = 1f;
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.PingPong(timer, fadeDuration) / fadeDuration;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (timer > fadeDuration * 2f)
        {
            Destroy(gameObject);
        }
    }
}
