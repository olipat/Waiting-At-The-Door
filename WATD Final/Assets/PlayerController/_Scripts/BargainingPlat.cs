using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BargainingPlatform : MonoBehaviour
{
    public enum PlatformType { Stable, Temporary }
    public enum PlatformVisualType { Horizontal, Vertical, HorizontalLarge }
    public PlatformVisualType visualType = PlatformVisualType.Horizontal;
    public PlatformType platformType = PlatformType.Stable;
    public event System.Action<BargainingPlatform> OnRebuilt;
    public Transform visualTransform;

    public Sprite brokenSprite;
    public Sprite rebuiltSprite;
    public Sprite[] breakingTransitionSprites;
    public Sprite[] rebuildingTransitionSprites;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool isRebuilt = false;
    private bool isBreaking = false;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (visualTransform == null){
            visualTransform = transform.Find("visual");
        }
        BreakPlatform();
    }

    public void Rebuild()
    {
        isRebuilt = true;
        StartCoroutine(PlayRebuildTransition());


        gameObject.layer = LayerMask.NameToLayer("platformLayer");

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

    private IEnumerator PlayRebuildTransition()
    {
        float frameDuration = 0.1f;

        for (int i = 0; i < rebuildingTransitionSprites.Length; i++)
        {
            sr.sprite = rebuildingTransitionSprites[i];
            yield return new WaitForSeconds(frameDuration);
        }

        sr.sprite = rebuiltSprite;
    }

    public void BreakPlatform()
    {
        isBreaking = true;
        isRebuilt = false;

        if (visualTransform != null){
            visualTransform.DOShakePosition(0.3f, new Vector3(0.1f, 0.1f, 0), 10, 90, false, true)
                       .SetRelative(true)
                       .OnComplete(() => StartCoroutine(PlayBreakTransition()));
        }
        else{
        StartCoroutine(PlayBreakTransition());
        }
    }

    private IEnumerator PlayBreakTransition()
    {
        gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");

        if (rb != null)
            rb.simulated = true;

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col != null)
            col.enabled = true;

        float frameDuration = 0.1f;
        for (int i = 0; i < breakingTransitionSprites.Length; i++)
        {
            sr.sprite = breakingTransitionSprites[i];
            yield return new WaitForSeconds(frameDuration);
        }

        sr.sprite = brokenSprite;
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
