using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

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
        FileStream file = new FileStream(Application.persistentDataPath + "/Save.dat", FileMode.OpenOrCreate);

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(file, UIController.Instance.saveStats);
        }
        catch(SerializationException e)
        {
            Debug.LogError("There was an issue serializing this data: " + e.Message);
        }
        finally
        {
            file.Close();
        }        
    }

    public void load()
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/Save.dat", FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();

            UIController.Instance.saveStats = formatter.Deserialize(file) as Stats;
        }catch(SerializationException e)
        {
            Debug.LogError("There was an issue deserializing this data: " + e.Message);
        }
        finally
        {
            file.Close();
        }
    }
}
