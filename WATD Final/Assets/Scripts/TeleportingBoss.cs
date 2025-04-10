using System.Collections;
using UnityEngine;

public class TeleportingBoss : MonoBehaviour
{
    public Transform[] teleportPoints; // Set these in the inspector
    //public float timeBetweenPhases = 2f;
    public GameObject spikeSet1;
    public GameObject spikeSet2;
    public GameObject fallingBlocks;

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
            //print("fi");
            yield return new WaitForSeconds(5f); // Stay visible
            //print("bitch");
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
        //print("PA");
        int attackType = Random.Range(0, 2); // Choose between attack 1 or 2

        if (attackType == 0)
        {
            //Instantiate(attack1Prefab, transform.position, Quaternion.identity);
            print("spike attack");
            int spikeSet = Random.Range(0, 2);
            if(spikeSet == 0)
            {
                //use spike set 1
                foreach (Transform child in spikeSet1.transform)
                {
                    child.GetComponent<spike>().attack();
                }
            }
            else
            {
                foreach (Transform child in spikeSet2.transform)
                {
                    child.GetComponent<spike>().attack();
                }
            }
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