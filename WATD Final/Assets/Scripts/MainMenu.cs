using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public string playScene;
    public TMP_Text title;
    public float fadeDuration = 1f;

    public RawImage cutsceneImage;         
    public CanvasGroup cutsceneGroup;     
    public VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMenuMusic();
        //StartCoroutine(FadeLoop());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        
        AudioManager.instance.PlaySFX(0);
        StartCoroutine(PlayCutsceneThenLoad());

    }

    IEnumerator PlayCutsceneThenLoad()
    {
        // Show the cutscene screen
        cutsceneImage.gameObject.SetActive(true);
        cutsceneGroup.alpha = 0;

        Debug.Log("Reached routine");
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            cutsceneGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }


        // Start the video and wait for it to finish
        videoPlayer.Play();
        AudioManager.instance.PlayCutsceneMusic(); 

        yield return new WaitForSeconds(0.3f);

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        AudioManager.instance.StopMusic();
        // Cutscene is done — now load the next level
        LevelLoader.Instance.LoadNextLevel();
    }


    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Quitting Game");

        AudioManager.instance.PlaySFX(0);
    }

    IEnumerator FadeLoop()
    {
        while (true) // Infinite loop for continuous fading
        {
            yield return StartCoroutine(FadeText(0f, 1f, fadeDuration)); // Fade in
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(FadeText(1f, 0f, fadeDuration)); // Fade out
            yield return new WaitForSeconds(fadeDuration);
        }
    }

    IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = title.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            title.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        // Ensure final value is exact
        title.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
