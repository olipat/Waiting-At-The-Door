using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    public Button[] levelButtons;
    public bool[] unlockedLevels; // Must match size of levelButtons

    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i; // capture for closure
            levelButtons[i].onClick.AddListener(() => OnLevelButtonClick(index));
        }
    }

    void OnLevelButtonClick(int index)
    {
        if (unlockedLevels[index])
        {
            LevelLoader.Instance.LoadThatLevel(index + 1); // Assuming level 1 = index 0
        }
        else
        {
            Debug.Log("Level " + (index + 1) + " is locked.");
        }
    }
}
