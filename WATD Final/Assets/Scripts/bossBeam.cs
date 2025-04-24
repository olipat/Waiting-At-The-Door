using UnityEngine;
using System.Collections;

public class bossBeam : MonoBehaviour
{
    public bool IsActive { get; private set; } = false;
    private Collider2D beamCollider;
    private SpriteRenderer spriteRenderer;

    private bool isPlayerInLight = false;
    private float timeInLight = 0f;
    //private bool isLightOn = true;

    public GameObject rollingBallPrefab; // The rolling ball prefab
    public Transform spawnPoint; // Where the ball will spawn

    public float maxTimeInLight = 2f; // Time before taking damage
    public float damageInterval = 0.5f; // Time between damage ticks
    public int damageAmount = 10;

    void Start()
    {
        beamCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        TurnOff();
    }

    public IEnumerator FlickerOn(float flickerDuration)
    {
        float flickerTime = 0f;
        while (flickerTime < flickerDuration)
        {
            SetBeamActive(Random.value > 0.5f);
            yield return new WaitForSeconds(0.1f);
            flickerTime += 0.1f;
        }
        SetBeamActive(true);
    }

    public void TurnOff()
    {
        SetBeamActive(false);
    }

    private void SetBeamActive(bool active)
    {
        IsActive = active;
        spriteRenderer.enabled = active;
        beamCollider.enabled = active;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInLight = true;
            timeInLight = 0f;
            StartCoroutine(DamagePlayerRoutine(other.gameObject));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInLight = false;
            StopCoroutine(DamagePlayerRoutine(other.gameObject));
        }
    }

    IEnumerator DamagePlayerRoutine(GameObject player)
    {
        while (isPlayerInLight)
        {
            timeInLight += damageInterval;
            if (timeInLight >= maxTimeInLight)
            {
                //spawn tire
                //Instantiate(rollingBallPrefab, spawnPoint.position, Quaternion.identity);
                Instantiate(rollingBallPrefab, spawnPoint.position, Quaternion.identity);
                timeInLight = 0;//reset time
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }

    public void ForceOff()
    {
        StopAllCoroutines(); // Cancel flickering or damage routines
        SetBeamActive(false); // Disables visuals and collider
        isPlayerInLight = false;
        timeInLight = 0f;
    }

}