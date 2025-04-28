using DG.Tweening;
using UnityEngine;

public class AngerBarkAbility : MonoBehaviour
{
    public float barkRange = 10f;
    public int damage = 1;
    public float cooldown = 5f; 
    public LayerMask enemyLayer;
    public LayerMask breakableLayer;

    public AudioClip barkclip;
    private AudioSource audioSource;
    [SerializeField] private Animator _animator; // Assign in Inspector
    private static readonly int AngerBarkTrigger = Animator.StringToHash("TriggerAngerBark");

    public GameObject barkFXobject;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void UseAngerBark()
    {
        Camera.main.transform.DOShakePosition(0.3f, strength: 0.5f, vibrato: 10, randomness: 90, snapping: false, fadeOut: true);
        transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 10, 1);

        if (_animator != null)
        {
            _animator.SetTrigger(AngerBarkTrigger);
        }
        StartCoroutine(AngerBark());
    }

    private System.Collections.IEnumerator AngerBark()
    {
        barkFXobject.GetComponent<abilityFX>().angerBark();
        if (barkclip != null && audioSource != null)
        {
            audioSource.clip = barkclip;
            audioSource.time = 0.5f;
            audioSource.pitch = 0.7f;
            audioSource.Play();

            float adjustedDuration = 1f / audioSource.pitch;
            yield return new WaitForSeconds(adjustedDuration);

            audioSource.Stop();
            audioSource.pitch = 1f;
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, barkRange, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Small Enemy"))
            {
                EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
                if (eh != null)
                {
                    eh.TakeDamage(damage);
                    Debug.Log(enemy.name + " took " + damage + " damage from Anger Bark");
                }
                
                SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.DOColor(Color.red, 0.1f).OnComplete(() => sr.DOColor(Color.white, 0.1f));
                }

            }
            else if (enemy.CompareTag("Boss"))
            {
                TeleportingBoss boss = enemy.GetComponent<TeleportingBoss>();
                if (boss != null)
                {
                    Debug.Log("Calling ShatterBarkHit on boss...");
                    boss.ShatterBarkHit();
                    Debug.Log(enemy.name + " hit by Shatter Bark!");
                }
                if (GameManager.instance.FightingBoss == true)
                {
                    SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.DOColor(Color.red, 0.1f).OnComplete(() => sr.DOColor(Color.white, 0.1f));
                    }
                }

            }
            else if (enemy.CompareTag("StoneEnemy"))
            {
                Debug.Log(enemy.name + " was hit by Shatter Bark!");

                stoneEnemy stone = enemy.GetComponent<stoneEnemy>();
                if (stone != null)
                {
                    stone.HandleShatterBark();
                }
                SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.DOColor(Color.red, 0.1f).OnComplete(() => sr.DOColor(Color.white, 0.1f));
                }
            }
        }

        Collider2D[] breakables = Physics2D.OverlapCircleAll(transform.position, barkRange, breakableLayer);
        foreach (Collider2D obj in breakables)
        {
            Debug.Log("Bark hit: " + obj.name);
            BreakableObject bo = obj.GetComponent<BreakableObject>();
            if (bo != null)
            {
                //obj.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 10, 1)
                    //.OnComplete(() => bo.Break());

                bo.Break();
                Debug.Log(obj.name + " was broken by Anger Bark");
                continue;
            }
            // Handle stalactites
            Stalactite stalactite = obj.GetComponentInParent<Stalactite>();
            if (stalactite != null)
            {
                stalactite.Shatter();
                Debug.Log(obj.name + " stalactite was shattered by Anger Bark");
            }

        }

        yield return null; 
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, barkRange);
    }
}

