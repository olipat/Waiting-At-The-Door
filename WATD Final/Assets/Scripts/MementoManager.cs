using UnityEngine;
using UnityEngine.UI;

public class MementoManager : MonoBehaviour
{
    public static MementoManager instance;

    public Image[] mementoSlots; 
    public GameObject[] mementos;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectMemento(int index)
    {
        if (index < mementoSlots.Length)
        {
            mementoSlots[index].color = Color.white; 

            UIController.Instance.saveStats.momentosCollected[index] = true;
        }
    }

    public void UncollectMemento(int index)
    {
        if (index < mementoSlots.Length)
        {
            mementoSlots[index].color = Color.black;
            mementos[index].SetActive(true);
        }
    }
}

