using UnityEngine;

public class PlatformGroupGateOpener : MonoBehaviour
{
    [Tooltip("Platforms that must all be rebuilt.")]
    public BargainingPlatform[] requiredPlatforms;

    [Tooltip("The object (like a gate) to activate once all are rebuilt.")]
    public GameObject gate;

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

    private void CheckPlatforms(BargainingPlatform _)
    {
        if (gateOpened) return;

        foreach (var platform in requiredPlatforms)
        {
            if (!platform.IsRebuilt())
                return;
        }

        Debug.Log("All platforms rebuilt — opening gate!");
        if (gate != null)
            gate.SetActive(true);

        gateOpened = true;
    }
}
