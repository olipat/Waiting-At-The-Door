using UnityEngine;

public class HealingBone : MonoBehaviour
{
    private bool playerInRange = false;

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
}
