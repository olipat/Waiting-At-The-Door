using UnityEngine;
using Controller;
using System.Collections;

public class BounceObj : MonoBehaviour
{
    public float bounceForce = 25f;
    public bool isGeyser = false;

    [Header("Geyser Settings")]
    public GameObject smokePrefab;
    public Transform smokeSpawnPoint;

    private bool canBounce = false;

    private void Start()
    {
        if (isGeyser)
        {
            StartCoroutine(GeyserCycle());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && (!isGeyser || canBounce))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TriggerBounce(bounceForce);
                Debug.Log("Bounce triggered!");
            }
        }
    }

    IEnumerator GeyserCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            SpawnSmoke();

            yield return new WaitForSeconds(0.5f);
            SpawnSmoke();

            yield return new WaitForSeconds(0.5f);
            SpawnSmoke();

            canBounce = true;
            for (int i = 0; i < 3; i++)
            {
                SpawnSmoke();
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1f);
            canBounce = false;
        }
    }

    void SpawnSmoke()
    {
        if (smokePrefab != null && smokeSpawnPoint != null)
        {
            Instantiate(smokePrefab, smokeSpawnPoint.position, Quaternion.identity);
        }
    }
}
