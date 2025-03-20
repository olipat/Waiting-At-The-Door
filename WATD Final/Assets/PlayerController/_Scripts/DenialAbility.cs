using UnityEngine;
using System.Collections.Generic;
using Controller;

public class DenialAbility : MonoBehaviour
{
    public static DenialAbility Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject platformPrefab; 
    public float platformLifetime = 5f;
    public float fallSpeed = 0.5f;
    public int maxPlatforms = 3;
    

    private List<GameObject> activePlatforms = new List<GameObject>();
    private bool facingRight = true; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("SpawnPlatform() triggered!");
            SpawnPlatform();
        }

        
        float moveInput = PlayerController.Instance.FrameInput.x;
        if (moveInput != 0)
            facingRight = moveInput > 0;
    }

    public void SpawnPlatform()
    {
        if (platformPrefab == null)
        {
            Debug.LogError("Platform Prefab not assigned in the Inspector!");
            return;
        }

        Debug.Log("Trying to spawn platform...");
        if (activePlatforms.Count >= maxPlatforms) return;

        Vector3 spawnOffset = facingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = transform.position + spawnOffset * 5f + Vector3.up * 0.5f;


        GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        activePlatforms.Add(newPlatform);
        Debug.Log("Spawned platform at: " + spawnPosition);

        StartCoroutine(HandlePlatformLifecycle(newPlatform));

    }

    System.Collections.IEnumerator HandlePlatformLifecycle(GameObject platform)
    {
        yield return new WaitForSeconds(platformLifetime);

        Rigidbody2D rb = platform.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = fallSpeed;
        }

        yield return new WaitForSeconds(5f);
        activePlatforms.Remove(platform);
        Destroy(platform);
    }

    public int GetPlatformCount()
    {
        return activePlatforms.Count;
    }
}
