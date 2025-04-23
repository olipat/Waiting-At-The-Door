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
        Debug.Log("Rebuild called on: " + gameObject.name);
        

        isRebuilt = true;
        sr.sprite = rebuiltSprite;
        Debug.Log("Setting sprite to: " + rebuiltSprite.name);

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
