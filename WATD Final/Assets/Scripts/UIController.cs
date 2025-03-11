using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Texture2D activeHealth, lostHealth;
    

    public GameObject cooldownWarning;

    public Image[] playerHealth;
    public Texture2D[] collectedMomentos, uncollectedMomentos;
    public Texture2D[] Ability1, Ability2, Ability3, Ability4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
