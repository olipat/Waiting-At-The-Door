using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    public static FallingPlatforms instance;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        
    }

    public FallingPlatform[] platforms;
    public Vector3[] initialPositions;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPositions = new Vector3[platforms.Length];

        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i] != null)
            {
                initialPositions[i] = platforms[i].transform.position;
            }

        }
    }

    public void returnPlatforms()
    {
        Debug.Log("Returning platforms");
        for (int i = 0;i < platforms.Length;i++)
        {
            if (platforms[i] != null)
            {
                platforms[i].gameObject.SetActive(true);
                platforms[i].ResetPlatform(initialPositions[i]);
            }
        }
    }
}
