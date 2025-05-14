using UnityEngine;
using UnityEngine.UI;

public class LevelSelectBackgroundController : MonoBehaviour
{
    [Header("Background Images (One per level)")]
    public GameObject[] backgrounds; // Assign one GameObject per level background

    public void ShowBackground(int index)
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].SetActive(i == index);
        }
    }

    public void HideAll()
    {
        foreach (var bg in backgrounds)
        {
            bg.SetActive(false);
        }
    }
}
