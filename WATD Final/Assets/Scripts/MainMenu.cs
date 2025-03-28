using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public string playScene;
    public TMP_Text title;
    public float fadeDuration = 1f;

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
        LevelLoader.Instance.LoadNextLevel();
        AudioManager.instance.PlaySFX(0);
        //AudioManager.instance.PlayBGM();
        
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
