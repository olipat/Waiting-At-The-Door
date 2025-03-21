using UnityEngine;
using Controller;

public class SniffAbility : MonoBehaviour
{
    public float sniffRange = 10f;
    public float glowDuration = 2f;
    public KeyCode sniffKey = KeyCode.Q;
    public LayerMask platformLayer;
    public LayerMask mementoLayer;

    void Update()
    {
        if (Input.GetKeyDown(sniffKey))
        {
            SniffForPlatforms();
        }
    }

    void SniffForPlatforms()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, sniffRange, platformLayer);
        foreach (Collider2D hit in hits)
        {
            FallingPlatform platform = hit.GetComponent<FallingPlatform>();
            if (platform != null)
            {
                platform.WarnPlatform(glowDuration);
            }
        }
        Collider2D[] mementoHits = Physics2D.OverlapCircleAll(transform.position, sniffRange, mementoLayer);

        foreach (Collider2D hit in mementoHits)
        {
            Memento memento = hit.GetComponent<Memento>();
            if (memento != null)
            {
                
                memento.GlowGold(glowDuration);
            }
        }
    }
}
