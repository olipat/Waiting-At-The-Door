using System.Collections;
using UnityEngine;

public class TeleportingBoss : MonoBehaviour
{
    public Transform[] teleportPoints; // Set these in the inspector
    public float timeBetweenPhases = 2f;
    public GameObject attack1Prefab;
    public GameObject attack2Prefab;

    private SpriteRenderer spriteRenderer;
    private Collider2D bossCollider;
    private bool isVulnerable = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossCollider = GetComponent<Collider2D>();
        StartCoroutine(BossLoop());
    }

    IEnumerator BossLoop()
    {
        while (true) // Loop until defeated
        {
            yield return new WaitForSeconds(timeBetweenPhases); // Stay visible
            isVulnerable = false;
            spriteRenderer.enabled = false;
            bossCollider.enabled = false;

            yield return new WaitForSeconds(0.5f); // Short delay before attack

            PerformAttack();

            yield return new WaitForSeconds(1f); // Attack duration

            Teleport();

            yield return new WaitForSeconds(0.5f); // Brief delay before reappearing

            spriteRenderer.enabled = true;
            bossCollider.enabled = true;
            isVulnerable = true;
        }
    }

    void Teleport()
    {
        int randomIndex = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[randomIndex].position;
    }

    void PerformAttack()
    {
        int attackType = Random.Range(0, 2); // Choose between attack 1 or 2

        if (attackType == 0)
        {
            //Instantiate(attack1Prefab, transform.position, Quaternion.identity);
            print("spike attack");
        }
        else
        {
            //Instantiate(attack2Prefab, transform.position, Quaternion.identity);
            print("falling rocks");
        }
    }

    public void TakeDamage()
    {
        if (isVulnerable)
        {
            Debug.Log("Boss takes damage!");
            // Add health reduction or death logic here
        }
    }
}