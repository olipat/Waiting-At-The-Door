using DG.Tweening;
using UnityEngine;

public class BarkAbility : MonoBehaviour
{
    public float barkRange = 10f;
    public float pushForce = 20f;
    public KeyCode barkKey = KeyCode.E;
    public LayerMask enemyLayer;
    public AudioClip barkClip;
    private AudioSource audioSource;
    [SerializeField] private Animator _animator; // Drag in Player's Animator in Inspector
    private static readonly int BarkTrigger = Animator.StringToHash("TriggerBark");

    public GameObject barkFXobject;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(barkKey) || Input.GetButtonDown("Fire3"))
        {
            transform.DOKill(true); // Kill any running tweens and complete instantly
            transform.localScale = Vector3.one; // Reset to original scale
            transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 8, 1);
            Bark();
        }
    }

    void Bark()
    {
        barkFXobject.GetComponent<abilityFX>().bark();
    
        if (_animator != null)
        {
            _animator.SetTrigger(BarkTrigger);
        }

         if (barkClip != null && audioSource != null)
        {
            StartCoroutine(PlayBarkForOneSecond());
        }
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, barkRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Small Enemy"))
            {
                //hit.transform.DOShakePosition(0.2f, strength: 0.2f, vibrato: 10, randomness: 90);

                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                Vector2 pushDir = (hit.transform.position - transform.position).normalized;

                StopSignEnemy sse = hit.GetComponent<StopSignEnemy>();
                if (sse != null)
                {
                    sse.ApplyKnockback(pushDir, pushForce);
                }
                else if (rb != null)
                {
                    rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
                }

               

            }
            else if (hit.CompareTag("StoneEnemy"))
            {
                Debug.Log(hit.name + " was hit by Shatter Bark!");

                stoneEnemy stone = hit.GetComponent<stoneEnemy>();
                if (stone != null)
                {
                    stone.HandleNormalBark();
                }
            }
        }
    }
    System.Collections.IEnumerator PlayBarkForOneSecond()
    {
        audioSource.clip = barkClip;
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        audioSource.Stop();
    }
}
