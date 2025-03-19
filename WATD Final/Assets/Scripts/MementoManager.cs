using UnityEngine;
using UnityEngine.UI;

public class MementoManager : MonoBehaviour
{
    public static MementoManager instance;

    public Image[] mementoSlots; 
    private int collectedCount = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectMemento()
    {
        if (collectedCount < mementoSlots.Length)
        {
            mementoSlots[collectedCount].color = Color.white; 
            collectedCount++;
        }
    }
}

