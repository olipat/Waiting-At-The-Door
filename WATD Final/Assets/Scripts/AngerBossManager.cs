using UnityEngine;
using UnityEngine.Tilemaps;

public class AngerBossManager : MonoBehaviour
{
    //Boss will have 6 armor spots (3 hits to head, 1 hit each to chest pieces)
    [Header("Boss Settings")]
    public int bossArmor = 5;
    //Use below for after water hits boss
    public Sprite solidifiedBossSprite;
    private SpriteRenderer bossSpriteRenderer;
    public TeleportingBoss teleportingBoss;
    public Transform finalBossPos;

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
    public Color solidifiedLavaColor = new Color(40f / 255f, 0f, 0f, 1f); 
    private Tilemap lavaTilemap;
    public Collider2D solidGroundCollider;

    [Header("Thwomps to Remove")]
    public GameObject thwomps;

    private bool stalactiteSpawned = false;
    private bool waterTriggered = false;

    private void Start()
    {
        bossSpriteRenderer = GetComponent<SpriteRenderer>();
        if (lavaObject != null)
            lavaTilemap = lavaObject.GetComponent<Tilemap>();

        if (solidGroundCollider != null)
            solidGroundCollider.enabled = false;
    }

    //spawn the stalactite once armor reaches 0 
    private void Update()
    {
        if (bossArmor <= 0 && !stalactiteSpawned)
        {
            Debug.Log("armor broken, triggering stalactites");
            HandleArmorBroken();
        }
    }

    private void HandleArmorBroken()
    {
        if (thwomps != null)
        {
            Debug.Log("thwomps disabled in boss room");
            thwomps.SetActive(false);
        }
        if (teleportingBoss != null)
        {
            teleportingBoss.attacksEnabled = false;
            teleportingBoss.StopAllCoroutines();
            teleportingBoss.enabled = false;

            if (finalBossPos != null)
                teleportingBoss.transform.position = finalBossPos.position;

            Rigidbody2D rb = teleportingBoss.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        SpawnStalactite();
    }

    //spawns in the stalactite after armor is borken and triggers paarticle sys when stalac borken
    private void SpawnStalactite()
    {
        Debug.Log("trying to spawn the boss room stalactite");
        if (stalactitePrefab != null && stalactiteSpawnPoint != null)
        {
            spawnedStalactite = Instantiate(stalactitePrefab, stalactiteSpawnPoint.position, Quaternion.identity);
            spawnedStalactite.transform.localScale = new Vector3(7f, 7f, 1f);
            stalactiteSpawned = true;
            Debug.Log("stalatite spawned");

            //stalactite was broken by player 
            Stalactite stalactiteScript = spawnedStalactite.GetComponent<Stalactite>();
            if (stalactiteScript != null)
            {
                Debug.Log("stlactie borken event calling");
                stalactiteScript.OnBroken += TriggerWaterSplash;
            }
        }
        else
        {
            Debug.LogWarning("Stalactite prefab or spawn point not assigned.");
        }
    }

    //The particle system is triggered here and the boss sprite and lava sprite are swapped
    private void TriggerWaterSplash()
    {
        Debug.Log("in the splash funtionc");
        if (waterTriggered) return; 

        waterTriggered = true;

         if (waterSplash != null)
        {
            Debug.Log("splash playing");
            waterSplash.Play();
        }

        if (bossSpriteRenderer != null && solidifiedBossSprite != null)
        {
            Debug.Log("boss sprite being changed");
            bossSpriteRenderer.sprite = solidifiedBossSprite;
            bossSpriteRenderer.color = new Color(40f, 0f, 0f, 255f);
        }
        if (lavaTilemap != null)
        {
            Debug.Log("lava sprite being changed");
            lavaTilemap.color = solidifiedLavaColor;
        }

        if (solidGroundCollider != null)
        {
            Debug.Log("lava collider enabled");
            solidGroundCollider.enabled = true;
        }
    }

    //Call this function when the player uses shatter bark on the boss armor
    //shatter bark might need to be modified for the boss so it doesn't destory 
    //all armor in one shot, but one at a time (olivia can handle this)
    public void DamageBossArmor(int amount)
    {
        bossArmor -= amount;
        bossArmor = Mathf.Max(bossArmor, 0);
        Debug.Log("Boss damaged");
    }
}
