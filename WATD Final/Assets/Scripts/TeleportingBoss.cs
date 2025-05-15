using System.Collections;
using UnityEngine;

public class TeleportingBoss : MonoBehaviour
{

    public Transform pointA; 
    public Transform pointB;
    private Coroutine attackRoutine;

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
        float attackTimer = 0f;
        while (true)
        {
            // RISE TO POINT A
            yield return StartCoroutine(MoveToPosition(pointA.position, 1f));
            spriteRenderer.enabled = true;
            bossCollider.enabled = true;
            isVulnerable = true;

            float vulnerableTime = 5f;
            float elapsedA = 0f;
            while (elapsedA < vulnerableTime)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= 5f)
                {
                    PerformAttack();
                    attackTimer = 0f;
                }

                elapsedA += Time.deltaTime;
                yield return null;
            }

            // FALL TO POINT B
            bossCollider.enabled = false;
            isVulnerable = false;
            yield return StartCoroutine(MoveToPosition(pointB.position, 1f));

            float idleTime = 10f;
            float elapsedB = 0f;
            while (elapsedB < idleTime)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= 5f)
                {
                    PerformAttack();
                    attackTimer = 0f;
                }

                elapsedB += Time.deltaTime;
                yield return null;
            }
        }
    }



    IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
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
                //child.GetComponent<thwompBoss>().fall();
                if (Random.value < 0.5f) // 50% chance
                {
                    child.GetComponent<thwompBoss>().fall();
                }
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
                if (manager.bossArmor > 0)
                {
                    manager.DamageBossArmor(1);
                }
                else if(manager.bossArmor == 0)
                {
                    ToastNotification.Show("He's too strong to finish head-on... but something�s shifted.", 4f, "error");
                }
            }
            else
            {
                Debug.LogWarning("AngerBossManager not found on boss!");
            }
        }
    }
}