using UnityEngine;

public class BargainingPlatform : MonoBehaviour
{
    public enum PlatformType { Stable, Temporary }
    public PlatformType platformType = PlatformType.Stable;

    public Sprite brokenSprite;
    public Sprite rebuiltSprite;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool isRebuilt = false;
    private bool isBreaking = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        BreakPlatform();
    }

    public void Rebuild()
    {
        if (isBreaking) return;

        isRebuilt = true;
        sr.sprite = rebuiltSprite;
        gameObject.layer = LayerMask.NameToLayer("Ground");

        if (platformType == PlatformType.Temporary)
        {
            Invoke(nameof(BreakPlatform), 3f); 
        }
    }

    public void BreakPlatform()
    {
        isBreaking = true;
        isRebuilt = false;
        sr.sprite = brokenSprite;
        gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
    }

    public bool IsRebuilt()
    {
        return isRebuilt;
    }

    public bool IsStable()
    {
        return platformType == PlatformType.Stable;
    }
}
