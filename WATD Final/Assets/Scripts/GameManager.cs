using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentLevel; // Auto-updated based on scene index
    public bool FightingBoss;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameController across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UpdateLevelNumber(); // Detect level at start
        SceneManager.sceneLoaded += OnSceneLoaded; // Auto-update when switching scenes
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateLevelNumber();
    }

    private void UpdateLevelNumber()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex; // Get scene index from Build Settings
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void LoadNextLevel()
    {
        LoadLevel(currentLevel + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        LoadLevel(0);
    }
}
