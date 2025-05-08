using System.Collections.Generic;
using UnityEngine;

public class BargainingAbility : MonoBehaviour
{
    public static BargainingAbility Instance;

    public float interactRange = 10f;
    public LayerMask platformLayer;
    public int maxStableRebuilt = 3;
    public int maxTemporaryRebuilt = 3;
    public float holdTimeToBreak = 0.5f;
    private float holdTimer = 0f;
    public bool unlocked = false;

    public GameObject ghostHorizontal;
    public GameObject ghostVertical;
    public GameObject ghostHorizontalLarge;
    public GameObject ghostBridge;

    private GameObject currentGhost;
    private Component currentTarget;

    private List<BargainingPlatform> stablePlatforms = new List<BargainingPlatform>();
    private List<BargainingPlatform> temporaryPlatforms = new List<BargainingPlatform>();

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

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

        Component bestCandidate = null;
        float bestScore = float.MinValue;

        foreach (var hit in hits)
        {
            var platform = hit.GetComponent<BargainingPlatform>();
            var bridge = hit.GetComponent<BridgePlatform>();

            if ((platform != null && !platform.IsRebuilt()) || (bridge != null && !bridge.IsRebuilt()))
            {
                Vector2 toPlatform = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
                float dot = Vector2.Dot(facingDirection, toPlatform);
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                float score = dot * 2f - distance;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestCandidate = (Component)(platform != null ? platform : bridge);
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
                GameObject ghostToSpawn = null;

                if (currentTarget is BargainingPlatform platform)
                {
                    switch (platform.visualType)
                    {
                        case BargainingPlatform.PlatformVisualType.Horizontal:
                            ghostToSpawn = ghostHorizontal;
                            break;
                        case BargainingPlatform.PlatformVisualType.Vertical:
                            ghostToSpawn = ghostVertical;
                            break;
                        case BargainingPlatform.PlatformVisualType.HorizontalLarge:
                            ghostToSpawn = ghostHorizontalLarge;
                            break;
                    }
                }
                else if (currentTarget is BridgePlatform)
                {
                    ghostToSpawn = ghostBridge;
                }

                if (ghostToSpawn != null)
                {
                    currentGhost = Instantiate(ghostToSpawn, currentTarget.transform.position, Quaternion.identity);
                }
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

            BridgePlatform bridge = hit.GetComponent<BridgePlatform>();
            BargainingPlatform platform = hit.GetComponent<BargainingPlatform>();

            if (bridge != null && !bridge.IsRebuilt())
            {
                if (score > bestScore)
                {
                    bestScore = score;
                    bestCandidate = bridge;
                }
            }
            else if (platform != null && !platform.IsRebuilt())
            {
                if (score > bestScore)
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



