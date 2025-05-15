using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public Animator transition;
    public float transitionTime = 1f;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AudioManager.instance.StopMusic();
            LoadNextLevel();
            
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1)); 

    }

    public void LoadThatLevel(int level)
    {
        AudioManager.instance.StopMusic();
        SceneManager.LoadScene(level);

    }

    IEnumerator LoadLevel(int levelIndex)
    {
        Debug.Log("Entered loadlevel: " + levelIndex);
        transition.SetTrigger("Start");

        AudioManager.instance.StopMusic();
        yield return new WaitForSeconds(transitionTime);

        
        SceneManager.LoadScene(levelIndex);

    }
}
