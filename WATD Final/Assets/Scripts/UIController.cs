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


    public GameObject endScreen;
    public bool playerDied = false;
    public float resultScreenDelayTime = 1f;

    public GameObject cooldownWarning;
    public float warningTime;
    private float warningCounter;

    public string mainMenuScene;

    public GameObject pauseScreen;

    public int playerHealth = 3, numHearts = 3;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Image[] abilities;
    public Sprite pathAvailable;
    public Sprite pathNotAvailable;

    public Sprite barkAvailable;
    public Sprite barkNotAvailable;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (warningCounter > 0)
        {
            warningCounter -= Time.deltaTime;

            if (warningCounter <= 0)
            {
                cooldownWarning.SetActive(false);
            }
        }



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            AudioManager.instance.PlaySFX(6);
            ApplyDamage();
        }


        UpdateHealthUI();
    }

    public void ShowWarning()
    {
        cooldownWarning.SetActive(true);
        warningCounter = warningTime;
    }

    public void MainMenu()
    {

        AudioManager.instance.PlaySFX(0);

        AudioManager.instance.PlayMenuMusic();

        SceneManager.LoadScene(mainMenuScene);

        Time.timeScale = 1f;

        

        
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1f;

        AudioManager.instance.PlaySFX(0);
    }


    public void PauseUnpause()
    {
        if (pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);

            Time.timeScale = 1f;
        }

        AudioManager.instance.PlaySFX(0);
    }

    public void ApplyDamage(int damageAmount = 1)
    {
        playerHealth -= damageAmount;
        playerHealth = Mathf.Clamp(playerHealth, 0, numHearts); // Ensure health doesn’t go below 0

        UpdateHealthUI();

        if (playerHealth <= 0)
        {
            PlayerDeath();
        }
    }

    public void RestoreHealth(int healAmount = 1)
    {
        playerHealth += healAmount;
        playerHealth = Mathf.Clamp(playerHealth, 0, numHearts); // Ensure health doesn't exceed max hearts

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (playerHealth > numHearts)
        {
            playerHealth = numHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            hearts[i].enabled = i < numHearts;
        }
    }

    public void PlayerDeath()
    {
        playerDied = true;
        StartCoroutine(ShowEndScreenCo());
    }

    IEnumerator ShowEndScreenCo()
    {
        yield return new WaitForSeconds(resultScreenDelayTime);

        CanvasGroup canvasGroup = endScreen.GetComponent<CanvasGroup>();
        endScreen.SetActive(true);

        float fadeDuration = 1f; 
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f; // Ensure it's fully visible
    }

}
