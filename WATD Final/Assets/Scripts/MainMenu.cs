using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public string playScene;
    public TMP_Text title;
    public float fadeDuration = 1f;

    public RawImage cutsceneImage;
    public CanvasGroup cutsceneGroup;
    public VideoPlayer videoPlayer;

    [Header("Level Select")]
    public GameObject levelSelectPanel;
    public Button startButton;
    public Button continueButton;
    public Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.Play();
        videoPlayer.Pause();
        AudioManager.instance.PlayMenuMusic();
        levelSelectPanel.SetActive(false);  // Ensure hidden on start
    }

    public void StartGame()
    {
        AudioManager.instance.PlaySFX(0);
        AudioManager.instance.StopMusic();
        StartCoroutine(PlayCutsceneThenLoad());
    }

    IEnumerator PlayCutsceneThenLoad()
    {
        yield return new WaitForSeconds(1f);

        cutsceneImage.gameObject.SetActive(true);
        cutsceneGroup.alpha = 0;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cutsceneGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        videoPlayer.Play();
        AudioManager.instance.PlayCutsceneMusic();
        yield return new WaitForSeconds(0.3f);

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        AudioManager.instance.StopMusic();
        LevelLoader.Instance.LoadNextLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
        AudioManager.instance.PlaySFX(0);
    }

    // === Level Select Methods ===

    public void OpenLevelSelect()
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        if (clicked != null)
        {
            clicked.transform.DOShakePosition(0.3f, new Vector3(10f, 10f, 0), 10, 90);
        }
        AudioManager.instance.PlaySFX(0);
        levelSelectPanel.SetActive(true);
        SetMainButtonsInteractable(false);
    }

    public void CloseLevelSelect()
    {
        AudioManager.instance.PlaySFX(0);
        levelSelectPanel.SetActive(false);
        SetMainButtonsInteractable(true);
    }

    private void SetMainButtonsInteractable(bool state)
    {
        startButton.interactable = state;
        continueButton.interactable = state;
        quitButton.interactable = state;
    }
}
