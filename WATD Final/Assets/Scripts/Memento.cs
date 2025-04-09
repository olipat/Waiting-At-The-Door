using System.Collections;
using UnityEngine;

public class Memento : MonoBehaviour
{
    public int index = 0;

    private bool playerInRange = false;
    private SpriteRenderer sr;
    private SpriteRenderer[] spriteRenderers;

    [Header("Glow Settings")]
    public GameObject glowPrefab;
    private GameObject activeGlow;

    void Update()
    {
        if (playerInRange)
        {
            AudioManager.instance.PlaySFX(8);
            PickupMemento();
        }
    }

    void PickupMemento()
    {
        MementoManager.instance.CollectMemento(index); 
        if (activeGlow != null)
        {
            activeGlow.SetActive(false); 
        }

        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void GlowGold(float duration)
    {
        StartCoroutine(GlowGoldTemporarily(duration));
    }

    private IEnumerator GlowGoldTemporarily(float duration)
    {
        if (glowPrefab != null)
        {
            if (activeGlow == null)
            {
                activeGlow = Instantiate(glowPrefab, transform.position, Quaternion.identity, transform);
                activeGlow.transform.localPosition = new Vector3(0f, 0f, 1f); 
            }
            else
            {
                activeGlow.SetActive(true); 
            }
        }

        yield return new WaitForSeconds(duration);

        if (activeGlow != null)
        {
            activeGlow.SetActive(false); 
        }
    }
}
