using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMLoader : MonoBehaviour
{
    public GameManager theGM;

    private void Awake()
    {
        if (FindFirstObjectByType<GameManager>() == null)
        {
            GameManager.instance = Instantiate(theGM);
            DontDestroyOnLoad(GameManager.instance.gameObject);
        }

    }
}
