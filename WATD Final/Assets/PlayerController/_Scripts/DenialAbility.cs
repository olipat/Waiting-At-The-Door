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
    public float platformLifetime = 3f;
    public float fallSpeed = 0.5f;
    public int maxPlatforms = 3;

    public AudioClip platformSpawnClip;
    private AudioSource audioSource;
    

    private List<GameObject> activePlatforms = new List<GameObject>();
    private bool facingRight = true; 


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Fire1"))
        {
            SpawnPlatform();
        }

        
        float moveInput = PlayerController.Instance.FrameInput.x;
        if (moveInput != 0)
            facingRight = moveInput > 0;
    }

    public void SpawnPlatform()
    {
        if (platformPrefab == null || Time.timeScale == 0)
        {
            return;
        }

        if (activePlatforms.Count >= maxPlatforms)
        {
            UIController.Instance.ShowPathWarning();
            return;
        }

        Vector3 spawnOffset = facingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPosition = transform.position + spawnOffset * 5f + Vector3.up * 0.5f;

        AudioManager.instance.PlaySFX(9);

        GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        activePlatforms.Add(newPlatform);
        activePlatforms.RemoveAll(platform => platform == null);

        if (platformSpawnClip != null)
        {
            audioSource.clip = platformSpawnClip;
            audioSource.Play();
        }
        //StartCoroutine(HandlePlatformLifecycle(newPlatform));

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

        yield return new WaitForSeconds(1f);
        activePlatforms.Remove(platform);
        Destroy(platform);
    }

    public int GetPlatformCount()
    {
        return activePlatforms.Count;
    }
    public void RemovePlatform(GameObject platform)
    {
        if (activePlatforms.Contains(platform)){
            activePlatforms.Remove(platform);
        }
    }
}

