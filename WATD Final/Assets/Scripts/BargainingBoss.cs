using UnityEngine;
using System.Collections;
using Controller;

public class BargainingBoss : MonoBehaviour
{
    public Animator animator;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float shootCooldown = 10f;
    private float shootTimer = 0f;

    public GameObject smallEnemyPrefab;
    public Transform[] spawnPoints;

    public GameObject[] stablePlatforms;

    public int maxHealth = 3;
    private int currentHealth;

    private bool secondPhase = false;
    private bool defeated = false;
    private bool active = false;

    public Sprite crumbleSprite;
    private SpriteRenderer sr;
    private Sprite originalSprite;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalSprite = sr.sprite;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        //gather stable platforms if not manually set
        if (stablePlatforms == null || stablePlatforms.Length == 0)
        {
            stablePlatforms = GetComponentsInChildren<GameObject>();
        }
    }

    void Update()
    {
        if (!active || defeated) return;

    }

    //wake up the boss 
    public void ActivateBoss()
    {
        active = true;
        StartCoroutine(PlayAriseThenIdle());
        StartCoroutine(BossShootingRoutine());
    }

    private IEnumerator BossShootingRoutine()
    {
        while (active && !defeated)
        {
            yield return new WaitForSeconds(shootCooldown);

            animator.Play("BBShoot"); 

            yield return new WaitForSeconds(0.1f);

            ShootProjectile();
            animator.Play("BBIdle");
        }
    }


    private IEnumerator PlayAriseThenIdle()
    {
        yield return new WaitForSeconds(2f);
        animator.Play("BBArise");

        yield return new WaitForSeconds(.5f);

        animator.Play("BBIdle");
    }
    // when the boss is hit, reduce health, call in new enemy, check phase
    public void TakeHit()
    {
        if (defeated || !active) return;

        currentHealth--;

        animator.Play("BBRoar");
        SummonEnemy();

        if (secondPhase && currentHealth <= 0)
        {
            // Cutscene trigger
            StartCoroutine(EndBossSequence());
        }
        else if (!secondPhase && currentHealth <= 0)
        {
            StartCoroutine(EnterSecondPhase());
        }
    }

    //Boss projectile shoot 
    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Vector2 dir = (FindObjectOfType<PlayerController>().transform.position - firePoint.position).normalized;
            proj.GetComponent<Rigidbody2D>().linearVelocity = dir * 6f;
        }
    }

    //summon in a small enemy
    void SummonEnemy()
    {
        if (smallEnemyPrefab != null && spawnPoints.Length > 0)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(smallEnemyPrefab, point.position, Quaternion.identity);
        }
    }

    //The stomp attack will break the platforms in the scene
    public void StompAttack()
    {
        animator.SetTrigger("BBStomp");
        foreach (GameObject platform in stablePlatforms)
        {
            if (platform != null && platform.TryGetComponent(out BargainingPlatform plat) && plat.IsStable() && plat.IsRebuilt())
            {
                plat.BreakPlatform();
            }
        }
    }

    //The boss is revived and rebuilt for round two 
    IEnumerator EnterSecondPhase()
    {
        active = false;
        sr.sprite = crumbleSprite;
        animator.enabled = false;
        yield return new WaitForSeconds(3f);

        // revive
        animator.enabled = true;
        animator.Play("BBArise");
        sr.sprite = originalSprite;
        currentHealth = 1;
        secondPhase = true;
        active = true;
    }

    //After revival + one hit, this would finally defeat the boss. 
    // remove the small enemies, boss sprite crumbles, cutscene logic
    IEnumerator EndBossSequence()
    {
        defeated = true;
        active = false;
        animator.SetTrigger("BBCrumble");

        yield return new WaitForSeconds(2f);

        //remove the small enemies
        foreach (var enemy in FindObjectsOfType<stoneEnemy>())
        {
            Destroy(enemy.gameObject);
        }

        // Cutscene logic here!!! after the first hit upon revival, journey
        //decides to stop fighting and small enemies vanish and boss crumbles

        sr.sprite = crumbleSprite;
        animator.enabled = false;
    }
}
