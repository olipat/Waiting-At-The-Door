using UnityEngine;

public class HealingBone : MonoBehaviour
{
    private bool playerInRange = false;
    private SpriteRenderer[] spriteRenderers;
    private GameObject glowBone;

    [Header("Glow Settings")]
    public GameObject boneprefab;


    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
    void Update()
    {
        if (playerInRange)
        {
            HealPlayer();
        }
    }

    void HealPlayer()
    {
        UIController.Instance.RestoreHealth(3); 
        AudioManager.instance.PlaySFX(8); 
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    public void ShowGlow(float duration)
    {
        if (boneprefab == null) return;

        if (glowBone == null)
        {
            glowBone = Instantiate(boneprefab, transform.position, Quaternion.identity, transform);
            glowBone.transform.localPosition = new Vector3(0, 0, 1); // Push slightly behind
        }
        else
        {
            glowBone.SetActive(true);
        }

        CancelInvoke(nameof(HideGlow));
        Invoke(nameof(HideGlow), duration);
    }

    void HideGlow()
    {
        if (glowBone != null)
        {
            glowBone.SetActive(false);
        }
    }
}
