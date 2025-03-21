using System.Collections;
using UnityEngine;

public class Memento : MonoBehaviour
{
    private bool playerInRange = false;
    private SpriteRenderer sr;
    private SpriteRenderer[] spriteRenderers;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            PickupMemento();
        }
    }

    void PickupMemento()
    {
        MementoManager.instance.CollectMemento(); 
        Destroy(gameObject); 
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
        Color originalColor = spriteRenderers[0].color;

        foreach (SpriteRenderer sr in spriteRenderers)
            sr.color = new Color(1f, 0.84f, 0f);

        yield return new WaitForSeconds(duration);

        foreach (SpriteRenderer sr in spriteRenderers)
            sr.color = originalColor;
    }
}
