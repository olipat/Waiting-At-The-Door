using UnityEngine;
using UnityEngine.UI;

public class BoneKeyManager : MonoBehaviour
{
    public static BoneKeyManager Instance;

    public Image[] keySlots;           
    public Sprite filledKeySprite;     
    private int collectedKeys = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectKey(int index)
    {
        if (index < keySlots.Length && keySlots[index] != null)
        {
            keySlots[index].sprite = filledKeySprite;
            keySlots[index].color = Color.white;
            collectedKeys++;
        }
    }

    public bool HasAllKeys()
    {
        return collectedKeys >= keySlots.Length;
    }
}


