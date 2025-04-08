using System.Collections.Generic;
using UnityEngine;

public class BargainingAbility : MonoBehaviour
{
    public float interactRange = 10f;
    public LayerMask platformLayer;
    public int maxStableRebuilt = 3;
    public int maxTemporaryRebuilt = 3;

    private List<BargainingPlatform> stablePlatforms = new List<BargainingPlatform>();
    private List<BargainingPlatform> temporaryPlatforms = new List<BargainingPlatform>();


    public void TryRebuildPlatform()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, platformLayer);

        foreach (var hit in hits)
        {
            BargainingPlatform platform = hit.GetComponent<BargainingPlatform>();
            if (platform != null && !platform.IsRebuilt())
            {
                RebuildPlatform(platform);
                break;
            }
        }
    }

    void RebuildPlatform(BargainingPlatform platform)
    {
        if (platform.IsStable())
        {
            if (stablePlatforms.Count >= maxStableRebuilt)
            {
                BargainingPlatform oldest = stablePlatforms[0];
                oldest.BreakPlatform();
                stablePlatforms.RemoveAt(0);
            }

            platform.Rebuild();
            stablePlatforms.Add(platform);
        }
        else
        {
            if (temporaryPlatforms.Count >= maxTemporaryRebuilt)
            {
                BargainingPlatform oldest = temporaryPlatforms[0];
                oldest.BreakPlatform();
                temporaryPlatforms.RemoveAt(0);
            }

            platform.Rebuild();
            temporaryPlatforms.Add(platform);
        }
    }
}


