using UnityEngine;

public class AngerBossManager : MonoBehaviour
{
    //Boss will have 6 armor spots (both arms, chest, 2 hits to head)
    [Header("Boss Settings")]
    public int bossArmor = 5;
    //Use below for after water hits boss
    public Sprite solidifiedBossSprite;
    private SpriteRenderer bossSpriteRenderer;

    //Below will be used to spawn a stalactite when the boss armor = 0 

    [Header("Stalactite Settings")]
    public GameObject stalactitePrefab;
    public Transform stalactiteSpawnPoint;
    private GameObject spawnedStalactite;

    //For the water like particle sys
    [Header("Water Splash")]
    public ParticleSystem waterSplash;

    //This will connect to the rising lava object I have for the boss room
    //After boss is defeated I will swap out sprite for solid ground and remove the deathzone
    [Header("Lava Settings")]
    public GameObject lavaObject; 
    public Sprite solidifiedLavaSprite;
    private SpriteRenderer lavaRenderer;

    private bool stalactiteSpawned = false;
    private bool waterTriggered = false;

    private void Start()
    {
        bossSpriteRenderer = GetComponent<SpriteRenderer>();
        if (lavaObject != null)
            lavaRenderer = lavaObject.GetComponent<SpriteRenderer>();
    }

    //spawn the stalactite once armor reaches 0 
    private void Update()
    {
        if (bossArmor <= 0 && !stalactiteSpawned)
        {
            SpawnStalactite();
        }
    }

    //spawns in the stalactite after armor is borken and triggers paarticle sys when stalac borken
    private void SpawnStalactite()
    {
        if (stalactitePrefab != null && stalactiteSpawnPoint != null)
        {
            spawnedStalactite = Instantiate(stalactitePrefab, stalactiteSpawnPoint.position, Quaternion.identity);
            stalactiteSpawned = true;

            //stalactite was broken by player 
            Stalactite stalactiteScript = spawnedStalactite.GetComponent<Stalactite>();
            if (stalactiteScript != null)
            {
                stalactiteScript.OnBroken += TriggerWaterSplash;
            }
        }
    }

    //The particle system is triggered here and the boss sprite and lava sprite are swapped
    private void TriggerWaterSplash()
    {
        if (waterTriggered) return; 

        waterTriggered = true;

        if (waterSplash != null)
            waterSplash.Play();

        if (bossSpriteRenderer != null && solidifiedBossSprite != null)
            bossSpriteRenderer.sprite = solidifiedBossSprite;

        if (lavaRenderer != null && solidifiedLavaSprite != null)
            lavaRenderer.sprite = solidifiedLavaSprite;

    }

    //Call this function when the player uses shatter bark on the boss armor
    //shatter bark might need to be modified for the boss so it doesn't destory 
    //all armor in one shot, but one at a time (olivia can handle this)
    public void DamageBossArmor(int amount)
    {
        bossArmor -= amount;
        bossArmor = Mathf.Max(bossArmor, 0);
    }
}
