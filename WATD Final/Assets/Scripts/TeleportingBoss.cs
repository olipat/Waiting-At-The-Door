using System.Collections;
using UnityEngine;

public class TeleportingBoss : MonoBehaviour
{
    public Transform[] teleportPoints; // Set these in the inspector
    public GameObject spikeSet1;
    public GameObject spikeSet2;
    public GameObject fallingBlocks;

    private SpriteRenderer spriteRenderer;
    private Collider2D bossCollider;
    private bool isVulnerable = true;
    [HideInInspector] public bool attacksEnabled = false;
    private Coroutine bossRoutine;

    public int health = 5;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossCollider = GetComponent<Collider2D>();
    }
    public void BeginBossFight()
    {
        if (bossRoutine == null)
        {
            attacksEnabled = true;
            bossRoutine = StartCoroutine(BossLoop());
        }
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
        if (!attacksEnabled) return;
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
            foreach (Transform child in fallingBlocks.transform)
            {
                child.GetComponent<thwompBoss>().fall();
            }
        }
    }

    public void TakeDamage()
    {
        if (isVulnerable)
        {
            Debug.Log("Boss takes damage!");
            health -= 1;
            if (health < 0)
            {
                Destroy(gameObject);
            }
            // Add health reduction or death logic here
        }
    }
    //If the boss isVulnerable and shatter bark hits, damage the boss
    //take it down 1 armor
    public void ShatterBarkHit()
    {
        if (isVulnerable)
        {
            AngerBossManager manager = GetComponentInParent<AngerBossManager>();
            if (manager != null)
            {
                manager.DamageBossArmor(1);
            }
            else
            {
                Debug.LogWarning("AngerBossManager not found on boss!");
            }
        }
    }
}