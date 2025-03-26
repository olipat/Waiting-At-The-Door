using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using Unity.VisualScripting;

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

    public Button saveButton;
    public Sprite buttonNotAvailable;
    private Sprite ButtonSprite;

    public Collider2D bossRoomBounds;
    public Collider2D cameraBounds;

    public GameObject endScreen;
    public bool playerDied = false;
    public float resultScreenDelayTime = 1f;

    public GameObject cooldownWarning;
    public float warningTime;
    private float warningCounter;

    public GameObject abilityWarning;
    public float abilityWarningTime;
    private float abilityWarningCounter;

    public GameObject pathWarning;
    public float pathWarningTime;
    private float pathWarningCounter;

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

    private CinemachineConfiner2D confiner;
    private CinemachineCamera vCam;
    private float zoomSize;

    public GameObject bossEntrance;
    private Vector3 bossEntrancePosition;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveGame();

        

        if (AudioManager.instance.playingBGM == false)
        {
            AudioManager.instance.PlayBGM();
        }

        vCam = FindFirstObjectByType<CinemachineCamera>(); // Get Cinemachine Camera
        confiner = vCam.GetComponent<CinemachineConfiner2D>(); // Get Camera Confiner
        zoomSize = vCam.Lens.OrthographicSize;

        ButtonSprite = saveButton.spriteState.selectedSprite;
       
        bossEntrancePosition = bossEntrance.transform.position;

        int abilityCount = abilities.Length;
        isOnCooldown = new bool[abilityCount];
        isUnlocked = new bool[abilityCount];
        abilityOutlines = new Outline[abilityCount];

        for(int i = 0; i < GameManager.instance.currentLevel; i++)
        {
            isUnlocked[i] = true;
        }
        
        

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

        if (Input.GetKeyDown(KeyCode.U))
        {
            UnlockAbility(1);
        }

        CheckButton();

        CheckCooldown();
        
        CheckAbilityWarning();

        CheckPathWarning();

        CheckAbilityActivation();

        UpdateHealthUI();
    }

    public void CheckButton()
    {
        if (GameManager.instance.FightingDenialBoss) 
        {
            saveButton.GetComponent<Image>().sprite = buttonNotAvailable;

            saveButton.enabled = false;
        }
        else
        {
            saveButton.GetComponent<Image>().sprite = ButtonSprite;

            saveButton.enabled = true;
        }
    }

    public void CheckCooldown()
    {
        if (warningCounter > 0)
        {
            warningCounter -= Time.deltaTime;

            if (warningCounter <= 0)
            {
                cooldownWarning.SetActive(false);
            }
        }
    }

    public void CheckAbilityWarning()
    {
        if (abilityWarningCounter > 0)
        {
            abilityWarningCounter -= Time.deltaTime;

            if (abilityWarningCounter <= 0)
            {
                abilityWarning.SetActive(false);
            }
        }
    }

    public void CheckPathWarning()
    {
        if (pathWarningCounter > 0)
        {
            pathWarningCounter -= Time.deltaTime;

            if (pathWarningCounter <= 0)
            {
                pathWarning.SetActive(false);
            }
        }
    }

    public void CheckAbilityActivation()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ActivateAbility(i);
            }
        }
    }

    public void ShowWarning()
    {
        abilityWarning.SetActive(false);
        pathWarning.SetActive(false);
        
        cooldownWarning.SetActive(true);
        warningCounter = warningTime;
    }

    public void ShowAbilityWarning()
    {
        cooldownWarning.SetActive(false);
        pathWarning.SetActive(false);

        abilityWarning.SetActive(true);
        abilityWarningCounter = abilityWarningTime;
    }

    public void ShowPathWarning()
    {
        cooldownWarning.SetActive(false);
        abilityWarning.SetActive(false);

        pathWarning.SetActive(true);
        pathWarningCounter = pathWarningTime;
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

    public void Unpause()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.instance.PlaySFX(0);
    }


    public void saveGame()
    {
        Debug.Log("Saving");
        saveStats.health = playerHealth;
        saveStats.level = GameManager.instance.currentLevel;
        saveStats.myPos.SetPos(player.transform.position);

        SaveManager.Instance.Save();
        Unpause();
    }

    public void saveGame(Vector3 position)
    {
        Debug.Log("Saving");
        saveStats.health = playerHealth;
        saveStats.level = GameManager.instance.currentLevel;
        saveStats.myPos.SetPos(position);

        SaveManager.Instance.Save();
        Unpause();
    }

    public void loadGame()
    {
        StopAllCoroutines();
        LevelLoader.Instance.transition.SetTrigger("Start");
        LevelLoader.Instance.transition.SetTrigger("End");
        SaveManager.Instance.load();
        Time.timeScale = 1f;

        Controller.PlayerController playerController = FindFirstObjectByType<Controller.PlayerController>();
        playerController.enabled = true;

        endScreen.SetActive(false);

        playerHealth = saveStats.health;
        GameManager.instance.currentLevel = saveStats.level; 
        player.transform.position = saveStats.myPos.GetPos();

        for (int i = 0; i < saveStats.momentosCollected.Length; i++)
        {
            if (saveStats.momentosCollected[i])
            {
                MementoManager.instance.CollectMemento(i);
            }
            else
            {
                MementoManager.instance.UncollectMemento(i);
            }
        }
        

        confiner.BoundingShape2D = cameraBounds;
        confiner.InvalidateBoundingShapeCache();

        vCam.Follow = player.transform;
        vCam.Lens.OrthographicSize = zoomSize;

        vCam.PreviousStateIsValid = false;

        bossEntrance.transform.position = bossEntrancePosition;
        BossRoomTrigger.Instance.inBossRoom = false;


    }

    public void ApplyDamage(int damageAmount = 1)
    {
        playerHealth -= damageAmount;
        playerHealth = Mathf.Clamp(playerHealth, 0, numHearts); // Ensure health doesn't go below 0

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
        Time.timeScale = 0f;
        canvasGroup.alpha = 1f; // Ensure it's fully visible
    }

    void ActivateAbility(int abilityIndex)
    {
        if (abilityIndex < 0 || abilityIndex >= abilities.Length) return;

        if (isUnlocked[abilityIndex] == false)
        {
            ShowAbilityWarning();
            return;
        }

        if (abilityIndex == 0)
        {
            DenialAbility denial = player.GetComponent<DenialAbility>();
            if (denial != null)
            {
                if (DenialAbility.Instance.GetPlatformCount() == 3)
                {
                    ShowPathWarning();
                    return;
                }
            }
            else
            {
                Debug.LogError("DenialAbility script not found on the player GameObject.");
                return;
            }
        }
        //Added anger bark here 
        else if (abilityIndex == 1)
        {
            if (isOnCooldown[abilityIndex] == true)
            {
                ShowWarning();
                return;
            }

            AngerBarkAbility anger = player.GetComponent<AngerBarkAbility>();
            if (anger != null)
            {
                anger.UseAngerBark();  
            }
            else
            {
                Debug.LogError("AngerBarkAbility script not found on the player GameObject.");
                return;
            }
        }
        else if (isOnCooldown[abilityIndex] == true)
        {
            ShowWarning();
            return;
        }
        if (abilityIndex != 0 && isUnlocked[abilityIndex])
        {
            Debug.Log("Entered Sprite Change");
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

    public void UnlockAbility(int abilityIndex)
    {
        isUnlocked[abilityIndex] = true;
        int spriteIndex = abilityIndex * 2;
        if (spriteIndex < abilitySprites.Length)
        {
            abilities[abilityIndex].sprite = abilitySprites[spriteIndex];
        }
    }
}