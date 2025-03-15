using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Save()
    {
        //Create a file
        FileStream file = new FileStream(Application.persistentDataPath + "/Save.dat", FileMode.OpenOrCreate);

        //Binary Formatter
        BinaryFormatter formatter = new BinaryFormatter();

        //Serization method
        formatter.Serialize(file, UIController.Instance.saveStats);

        file.Close();
    }

    public void load()
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/Save.dat", FileMode.Open);
        
        BinaryFormatter formatter = new BinaryFormatter();
        
        UIController.Instance.saveStats = formatter.Deserialize(file) as Stats;

        file.Close();
    }
}
