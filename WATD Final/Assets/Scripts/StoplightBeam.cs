using System.Collections;
using UnityEngine;

public class StoplightBeam : MonoBehaviour
{
    public float maxTimeInLight = 2f; // Time before taking damage
    public float damageInterval = 0.5f; // Time between damage ticks
    public int damageAmount = 10;

    public SpriteRenderer beamRenderer; // Reference to the red light beam sprite
    public Collider2D lightCollider; // The collider for detection

    public float minFlickerTime = 0.1f;
    public float maxFlickerTime = 0.5f;
    public float minOnTime = 3f;
    public float maxOnTime = 5f;
    public float minOffTime = 2f;
    public float maxOffTime = 4f;

    private bool isPlayerInLight = false;
    private float timeInLight = 0f;
    private bool isLightOn = true;

    public GameObject rollingBallPrefab; // The rolling ball prefab
    public Transform spawnPoint; // Where the ball will spawn

    void Start()
    {
        StartCoroutine(LightControlRoutine());
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
                GameObject tire = Instantiate(rollingBallPrefab, spawnPoint.position, Quaternion.identity);
                tire.GetComponent<Projectile>().direction = 1;
                print("player was in light too long and should take damage or spawn enemy");
                timeInLight = 0;//reset time
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }

    IEnumerator LightControlRoutine()
    {
        while (true)
        {
            //flicker before turning off
            float flickerDuration = Random.Range(minFlickerTime, maxFlickerTime);
            for (int i = 0; i < Random.Range(3, 6); i++)
            {
                ToggleLight();
                yield return new WaitForSeconds(flickerDuration);
            }

            //light off for a random duration
            SetLightState(false);
            yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));

            //flicker before turning on
            for (int i = 0; i < Random.Range(3, 6); i++)
            {
                ToggleLight();
                yield return new WaitForSeconds(flickerDuration);
            }

            //light on for a random duration
            SetLightState(true);
            yield return new WaitForSeconds(Random.Range(minOnTime, maxOnTime));
        }
    }

    void ToggleLight()
    {
        SetLightState(!isLightOn);
    }

    void SetLightState(bool state)
    {
        isLightOn = state;
        beamRenderer.enabled = state;
        lightCollider.enabled = state;
    }
}
