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
        if(Instance == null)
        {
            Instance = this;
        }
        
    }

    public Stats saveStats;
    public GameObject player;

    public GameObject endScreen;
    public bool playerDied = false;
    public float resultScreenDelayTime = 1f;

    public GameObject cooldownWarning;
    public float warningTime;
    private float warningCounter;

    public GameObject abilityWarning;
    public float abilityWarningTime;
    private float abilityWarningCounter;

    public string mainMenuScene;

    public GameObject pauseScreen;

    public int playerHealth = 3, numHearts = 3;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Image[] abilities;
    public TMP_Text[] cooldownTexts; // Array for cooldown texts
    public Sprite[] abilitySprites; // Holds available/not available sprites: [0=available(1st), 1=notAvailable(1st), 2=available(2nd), etc.]
    public float[] cooldownTimes; // Cooldown duration per ability

    private bool[] isOnCooldown; // Track cooldown for each ability
    private Outline[] abilityOutlines; // Store outline components
    private bool[] isUnlocked;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveGame();

        if(AudioManager.instance.playingBGM == false)
        {
            AudioManager.instance.PlayBGM();
        }
            

        int abilityCount = abilities.Length;
        isOnCooldown = new bool[abilityCount];
        isUnlocked = new bool[abilityCount];
        abilityOutlines = new Outline[abilityCount];

        isUnlocked[0] = true;
        isUnlocked[1] = true;

        for (int i = 0; i < abilityCount; i++)
        {
            // Ensure each ability has an outline
            abilityOutlines[i] = abilities[i].GetComponent<Outline>();
            if (abilityOutlines[i] == null)
            {
                abilityOutlines[i] = abilities[i].gameObject.AddComponent<Outline>();
            }

            // Set the outline properties (Effect Distance & Effect Color)
            abilityOutlines[i].effectDistance = new Vector2(2, 2);
            abilityOutlines[i].effectColor = new Color32(255, 0, 0, 200);
            abilityOutlines[i].enabled = false; // Initially disable outlines

            // Ensure cooldown text starts hidden
            if (cooldownTexts[i] != null)
            {
                cooldownTexts[i].enabled = false;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {

            ApplyDamage();
            AudioManager.instance.PlaySFX(6);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }


        if (warningCounter > 0)
        {
            warningCounter -= Time.deltaTime;

            if (warningCounter <= 0)
            {
                cooldownWarning.SetActive(false);
            }
        }

        if (abilityWarningCounter > 0)
        {
            abilityWarningCounter -= Time.deltaTime;

            if (abilityWarningCounter <= 0)
            {
                abilityWarning.SetActive(false);
            }
        }


        for (int i = 0; i < abilities.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ActivateAbility(i);
            }
        }


        UpdateHealthUI();
    }

    public void ShowWarning()
    {
        abilityWarning.SetActive(false);
        
        cooldownWarning.SetActive(true);
        warningCounter = warningTime;
    }

    public void ShowAbilityWarning()
    {
        cooldownWarning.SetActive(false);

        abilityWarning.SetActive(true);
        abilityWarningCounter = abilityWarningTime;
    }

    public void MainMenu()
    {

        SceneManager.LoadScene(mainMenuScene);

        Time.timeScale = 1f;

        AudioManager.instance.PlaySFX(0);

        if(AudioManager.instance.playingMenuMusic == false)
        {
            AudioManager.instance.PlayMenuMusic();
        }
        




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


    public void saveGame()
    {
        Debug.Log("Saving");
        saveStats.health = playerHealth;
        saveStats.level = GameManager.instance.currentLevel;
        saveStats.myPos.SetPos(player.transform.position);

        SaveManager.Instance.Save();
        
    }

    public void loadGame()
    {
        SaveManager.Instance.load();
        
        endScreen.SetActive(false);

        playerHealth = saveStats.health;
        GameManager.instance.currentLevel = saveStats.level; 
        player.transform.position = saveStats.myPos.GetPos();
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

    void ActivateAbility(int abilityIndex)
    {
        if (abilityIndex < 0 || abilityIndex >= abilities.Length) return;
        
        if (isOnCooldown[abilityIndex] == true)
        {
            ShowWarning(); 
            return;
        }

        if (isUnlocked[abilityIndex] == false)
        {
            ShowAbilityWarning();
            return;
        }

        // Change sprite using the new index logic (2i = available, 2i+1 = not available)
        int spriteIndex = abilityIndex * 2 + 1;
        if (spriteIndex < abilitySprites.Length)
        {
            abilities[abilityIndex].sprite = abilitySprites[spriteIndex];
        }

        // Enable outline
        abilityOutlines[abilityIndex].enabled = true;

        // Start cooldown coroutine
        StartCoroutine(AbilityCooldown(abilityIndex));
    }

    IEnumerator AbilityCooldown(int abilityIndex)
    {
        isOnCooldown[abilityIndex] = true;
        float cooldownTime = cooldownTimes[abilityIndex];

        if (cooldownTexts[abilityIndex] != null)
        {
            cooldownTexts[abilityIndex].gameObject.SetActive(true); // Ensure text is visible
            cooldownTexts[abilityIndex].enabled = true;

            for (int i = (int)cooldownTime; i > 0; i--)
            {
                cooldownTexts[abilityIndex].text = i.ToString(); // Update text each second
                yield return new WaitForSeconds(1f);
            }

            cooldownTexts[abilityIndex].enabled = false;
            cooldownTexts[abilityIndex].gameObject.SetActive(false); // Hide after cooldown
        }

        // Reset ability to "Available" sprite
        int spriteIndex = abilityIndex * 2;
        if (spriteIndex < abilitySprites.Length)
        {
            abilities[abilityIndex].sprite = abilitySprites[spriteIndex];
        }

        // Disable outline when cooldown ends
        abilityOutlines[abilityIndex].enabled = false;

        isOnCooldown[abilityIndex] = false;
    }
}
