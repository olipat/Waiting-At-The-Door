using UnityEngine;
using UnityEngine.UI;

public class BoneKeyManager : MonoBehaviour
{
    public static BoneKeyManager Instance;

    public Image[] keySlots;             
    public GameObject[] boneKeyObjects;  

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectKey(int index)
    {
        if (index < keySlots.Length)
        {
            keySlots[index].color = Color.white;
            UIController.Instance.saveStats.boneKeysCollected[index] = true;
        }
    }

    public void UncollectKey(int index)
    {
        if (index < keySlots.Length)
        {
            keySlots[index].color = Color.black;
            boneKeyObjects[index].SetActive(true);
        }
    }


    public bool HasAllKeys()
    {
        foreach (Image slot in keySlots)
        {
            if (slot.color != Color.white)
                return false;
        }
        return true;
    }
}


