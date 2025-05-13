using UnityEngine;
using Controller;
using DG.Tweening;

public class SniffAbility : MonoBehaviour
{
    public float sniffRange = 10f;
    public float glowDuration = 2f;
    public KeyCode sniffKey = KeyCode.Q;
    public LayerMask platformLayer;
    public LayerMask mementoLayer;

    [SerializeField] private Animator _animator; // Drag PlayerAnimator's Animator here in Inspector

    private static readonly int SniffTrigger = Animator.StringToHash("TriggerSniff");

    public GameObject barkFXobject;
    void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Fire2"))
        {
            AudioManager.instance.PlaySFX(11);
            if (_animator != null)
            {
                _animator.SetTrigger(SniffTrigger);
            }

            transform.DOKill(true); // Kill any running tweens and complete instantly
            transform.localScale = Vector3.one; // Reset to original scale
            transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 8, 1);

            SniffForPlatforms();
        }
    }

    void SniffForPlatforms()
    {
        barkFXobject.GetComponent<abilityFX>().sniff();
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
