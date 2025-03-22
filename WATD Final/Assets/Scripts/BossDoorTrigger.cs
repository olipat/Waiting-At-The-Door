using UnityEngine;

public class BossEntranceTrigger : MonoBehaviour
{
    public GameObject blockadeWall; // Assign in Inspector
    public string playerTag = "Player";
    public float delayBeforeBlockade = 5f; // Delay time in seconds

    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag(playerTag))
        {
            triggered = true;
            StartCoroutine(TriggerBlockadeAfterDelay());

            GetComponent<Collider2D>().enabled = false;
        }
    }

    System.Collections.IEnumerator TriggerBlockadeAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeBlockade);

        if (blockadeWall != null)
        {
            blockadeWall.SetActive(true);
        }

        //this would destroy the trigger if it's only used once, but we'll
        //have to consider how to change this given the player might fail the boss fight
        Destroy(gameObject);
    }
}

