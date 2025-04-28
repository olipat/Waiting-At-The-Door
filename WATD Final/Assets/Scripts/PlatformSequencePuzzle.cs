using UnityEngine;

public class PlatformGroupGateOpener : MonoBehaviour
{
    public BargainingPlatform[] requiredPlatforms;

    public GameObject gate;

    public Sprite openGate;
    private SpriteRenderer gateRenderer;
    private Collider2D gateCollider;

    private bool gateOpened = false;

    private void OnEnable()
    {
        foreach (var platform in requiredPlatforms)
        {
            platform.OnRebuilt += CheckPlatforms;
        }
    }

    private void OnDisable()
    {
        foreach (var platform in requiredPlatforms)
        {
            platform.OnRebuilt -= CheckPlatforms;
        }
    }
    private void Start()
    {
        if (gate != null)
        {
            gateRenderer = gate.GetComponent<SpriteRenderer>();
            gateCollider = gate.GetComponent<Collider2D>();
        }
    }
    private void CheckPlatforms(BargainingPlatform _)
    {
        if (gateOpened) return;

        foreach (var platform in requiredPlatforms)
        {
            if (!platform.IsRebuilt())
                return;
        }

        OpenGate();
    }
     private void OpenGate()
    {
        if (gate != null)
        {
            if (openGate != null && gateRenderer != null)
            {
                gateRenderer.sprite = openGate; 
            }

            if (gateCollider != null)
            {
                gateCollider.enabled = false; 
            }

            Debug.Log("Gate opened!");
        }

        gateOpened = true;
    }
}
