using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class denialBoss : MonoBehaviour
{
    public Transform[] targetLocations; // Array of light positions
    private List<int> activeLights = new List<int>(); // Stores currently active lights
    private bool loopFlag = false;

    void Start()
    {
        StartCoroutine(BossCoroutine());
    }

    void Update()
    {
        if (loopFlag)
        {
            loopFlag = false;
            StartCoroutine(BossCoroutine());
        }
    }

    IEnumerator BossCoroutine()
    {
        // Clear previously active lights
        activeLights.Clear();

        // Determine how many lights to turn on (e.g., 2 to 5 lights)
        int numLightsToActivate = Random.Range(2, 6);

        // Pick random unique lights
        while (activeLights.Count < numLightsToActivate)
        {
            int randomLight = Random.Range(0, targetLocations.Length);
            if (!activeLights.Contains(randomLight))
            {
                activeLights.Add(randomLight);
            }
        }

        // Print active lights (replace with actual light activation)
        foreach (int lightIndex in activeLights)
        {
            print("Shine light on point " + lightIndex);
            // You can replace this with actual light activation logic
        }

        // Wait for 5 seconds before changing lights again
        yield return new WaitForSeconds(5);

        loopFlag = true;
    }
}
