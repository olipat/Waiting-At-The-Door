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
    public GameObject ghostPrefab;
    private GameObject currentGhost;
    private BargainingPlatform currentTarget;

    private List<BargainingPlatform> stablePlatforms = new List<BargainingPlatform>();
    private List<BargainingPlatform> temporaryPlatforms = new List<BargainingPlatform>();

    void Update()
    {
     if (unlocked && Time.timeScale != 0)
        {
            ShowRebuildPreview();
            UseBargainingAbility();
        }   
    }
    private void ShowRebuildPreview()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, platformLayer);
        Vector2 facingDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;
        if (facingDirection == Vector2.zero)
            facingDirection = Vector2.right;

        BargainingPlatform bestCandidate = null;
        float bestScore = float.MinValue;

        foreach (var hit in hits)
        {
            BargainingPlatform platform = hit.GetComponent<BargainingPlatform>();
            if (platform != null && !platform.IsRebuilt())
            {
                Vector2 toPlatform = ((Vector2)platform.transform.position - (Vector2)transform.position).normalized;
                float dot = Vector2.Dot(facingDirection, toPlatform);
                float distance = Vector2.Distance(transform.position, platform.transform.position);
                float score = dot * 2f - distance;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestCandidate = platform;
                }
            }
        }

        if (bestCandidate != currentTarget)
        {
            currentTarget = bestCandidate;

            if (currentGhost != null)
                Destroy(currentGhost);

            if (currentTarget != null)
            {
                currentGhost = Instantiate(ghostPrefab, currentTarget.transform.position, Quaternion.identity);
            }
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
        Debug.Log("trying to rebuild");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, platformLayer);
        Debug.Log("Found " + hits.Length + " colliders in range");

        Vector2 facingDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;
        if (facingDirection == Vector2.zero)
            facingDirection = Vector2.right;

        float bestScore = float.MinValue;
        Component bestCandidate = null;

        foreach (var hit in hits)
        {
            Vector2 toTarget = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
            float dot = Vector2.Dot(facingDirection, toTarget);
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            float score = dot * 2f - distance;

            if (score > bestScore)
            {
                // Check if it's a bridge or platform
                BridgePlatform bridge = hit.GetComponent<BridgePlatform>();
                BargainingPlatform platform = hit.GetComponent<BargainingPlatform>();

                if (bridge != null && !bridge.IsRebuilt())
                {
                    bestScore = score;
                    bestCandidate = bridge;
                }
                else if (platform != null && !platform.IsRebuilt())
                {
                    bestScore = score;
                    bestCandidate = platform;
                }
            }
        }

        if (bestCandidate is BridgePlatform bridgeTarget)
        {
            bridgeTarget.Rebuild();
        }
        else if (bestCandidate is BargainingPlatform platformTarget)
        {
            RebuildPlatform(platformTarget);
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
            Debug.Log("calling platform rebuild next");
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


