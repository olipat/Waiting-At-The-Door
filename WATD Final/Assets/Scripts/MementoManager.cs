using UnityEngine;
using UnityEngine.UI;

public class MementoManager : MonoBehaviour
{
    public static MementoManager instance;

    public Image[] mementoSlots; 
    public GameObject[] mementos;
    public string[] textArr;

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

            if(index < textArr.Length)
            ToastNotification.Show(textArr[index], 4f);
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

