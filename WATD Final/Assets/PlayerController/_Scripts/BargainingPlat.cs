using UnityEngine;

public class BargainingPlatform : MonoBehaviour
{
    public enum PlatformType { Stable, Temporary }
    public PlatformType platformType = PlatformType.Stable;
    public event System.Action<BargainingPlatform> OnRebuilt;

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
        isRebuilt = true;
        sr.sprite = rebuiltSprite;

        gameObject.layer = LayerMask.NameToLayer("ground");

        if (rb != null){
            rb.simulated = true;
        }

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null){
            col.enabled = true;
            Debug.Log("Collider enabled on: " + gameObject.name);
        }
        isBreaking = false;

        OnRebuilt?.Invoke(this);

        if (platformType == PlatformType.Temporary)
        {
            Invoke(nameof(BreakPlatform), 3f); 
            Debug.Log("Set to break again in 3s");
        }
    }

    public void BreakPlatform()
    {
        isBreaking = true;
        isRebuilt = false;
        sr.sprite = brokenSprite;
        gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");

        if (rb != null)
            rb.simulated = true;

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
            col.enabled = true;
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
