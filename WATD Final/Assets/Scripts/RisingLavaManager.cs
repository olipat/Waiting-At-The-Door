using UnityEngine;

public class RisingLavaManager : MonoBehaviour
{
    public static RisingLavaManager instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public RisingLava[] lavaZones;
    public GameObject[] lavaTriggers;

    public void ResetAllLava()
    {
        Debug.Log("Resetting Lava Manager");
        for (int i = 0; i < lavaZones.Length; i++)
        {
            if (lavaZones[i] != null)
            {
                lavaZones[i].RetreatLava();
            }
        }

        foreach (GameObject obj in lavaTriggers)
        {
            obj.SetActive(true);

            BoxCollider2D col = obj.GetComponent<BoxCollider2D>();
            if (col != null)
            {
                col.enabled = true;
                
            }
        }

    }
}
