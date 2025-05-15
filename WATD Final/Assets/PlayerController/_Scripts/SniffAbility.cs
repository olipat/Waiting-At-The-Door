using UnityEngine;
using Controller;
using DG.Tweening;
using Unity.VisualScripting;

public class SniffAbility : MonoBehaviour
{
    public float sniffRange = 10f;
    public float glowDuration = 2f;
    public KeyCode sniffKey = KeyCode.Q;
    public LayerMask platformLayer;
    public LayerMask mementoLayer;

    public GameObject redCaution;
    public GameObject yellowCaution;
    public GameObject greenCaution;

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

    void HideAllSymbols()
    {
        redCaution.SetActive(false);
        yellowCaution.SetActive(false);
        greenCaution.SetActive(false);
    }


    void SniffForPlatforms()
    {
        barkFXobject.GetComponent<abilityFX>().sniff();

        Collider2D[] sniffHits = Physics2D.OverlapCircleAll(transform.position, sniffRange);
        foreach (Collider2D hit in sniffHits)
        {
            
            GameObject obj = hit.gameObject;
            string name = obj.name.ToLower();
            Vector3 spawnPos = obj.transform.position + Vector3.up * 1.5f;
            Debug.Log("Detected object: " + obj.name + " | Tag: " + obj.tag);

            BargainingPlatform platform = obj.GetComponent<BargainingPlatform>();
        if (platform != null)
            {
                if (platform.platformType == BargainingPlatform.PlatformType.Temporary)
                {
                    Instantiate(yellowCaution, spawnPos, Quaternion.identity);
                }
                else if (platform.platformType == BargainingPlatform.PlatformType.Stable)
                {
                    Instantiate(greenCaution, spawnPos, Quaternion.identity);
                }
                continue; 
            }
                if (name.Contains("alarmo") || name.Contains("sleepingstone") || name.Contains("fallingplatform1") || name.Contains("lava") || name.Contains("thwomp") || name.Contains("exploding") || name.Contains("foreground"))
            {
                Instantiate(redCaution, spawnPos, Quaternion.identity);
            }
            else if (name.Contains("lightleft") || name.Contains("oneway") || obj.CompareTag("sentry") || name.Contains("smalllightenemy") || name.Contains("stopsignguy") || name.Contains("lightright"))
            {
                Instantiate(yellowCaution, spawnPos, Quaternion.identity);
            }
            else if (name.Contains("trampoline") || name.Contains("sleepingenemyonly")  || name.Contains("boulder") || name.Contains("geyser") || name.Contains("obstaclebox") || name.Contains("fastballer_0") || name.Contains("bottom"))
            {
                Instantiate(greenCaution, spawnPos, Quaternion.identity);
            }

            // Existing memento glow (unchanged)
            if (((1 << obj.layer) & mementoLayer) != 0)
            {
                Memento memento = obj.GetComponent<Memento>();
                if (memento != null)
                {
                    memento.GlowGold(glowDuration);
                }
            }
        }
    }

}
