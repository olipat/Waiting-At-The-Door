using UnityEngine;

public class BossEntranceTrigger : MonoBehaviour
{
    public GameObject blockadeWall; // Assign in Inspector
    public string playerTag = "Player";
    public float delayBeforeBlockade = 5f;

    public float moveDistance = 5f;      
    public float moveDuration = 1f;

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

            // Start drop animation
            Vector3 startPos = blockadeWall.transform.position;
            Vector3 endPos = startPos + Vector3.down * moveDistance;

            float elapsed = 0f;
            while (elapsed < moveDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / moveDuration);
                blockadeWall.transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }

            blockadeWall.transform.position = endPos;
        }

        Destroy(gameObject);
    }
}

