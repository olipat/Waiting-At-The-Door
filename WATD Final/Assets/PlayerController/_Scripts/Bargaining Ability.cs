using System.Collections.Generic;
using UnityEngine;

public class BargainingAbility : MonoBehaviour
{
    public static BargainingAbility Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public float interactRange = 10f;
    public LayerMask platformLayer;
    public int maxStableRebuilt = 3;
    public int maxTemporaryRebuilt = 3;
    public float holdTimeToBreak = 0.5f; 
    private float holdTimer = 0f;
    public bool unlocked = false;

    private List<BargainingPlatform> stablePlatforms = new List<BargainingPlatform>();
    private List<BargainingPlatform> temporaryPlatforms = new List<BargainingPlatform>();

    void Update()
    {
     if (unlocked && Time.timeScale != 0)
        {
            UseBargainingAbility();
        }   
    }

    public void UseBargainingAbility()
    {
        
        if (Input.GetKey(KeyCode.Alpha3))
        {
            
            holdTimer += Time.deltaTime;

            if (holdTimer > holdTimeToBreak)
            {
                TryBreakPlatform();
                holdTimer = 0f;
            }
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            Debug.Log("Using bargaining build Ability");
            if (holdTimer <= holdTimeToBreak && holdTimer > 0f)
            {
                TryRebuildPlatform();
            }
            holdTimer = 0f;
        }
    }

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

    public void TryBreakPlatform()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, platformLayer);

        foreach (var hit in hits)
        {
            BargainingPlatform platform = hit.GetComponent<BargainingPlatform>();
            if (platform != null && platform.IsRebuilt()) 
            {
                platform.BreakPlatform();
                stablePlatforms.Remove(platform);
                temporaryPlatforms.Remove(platform);
                Debug.Log("Manually broke platform: " + platform.name);
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


