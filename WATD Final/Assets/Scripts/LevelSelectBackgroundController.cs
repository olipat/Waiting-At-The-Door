using UnityEngine;
using UnityEngine.UI;

public class LevelSelectBackgroundSwapper : MonoBehaviour
{
    [Header("Background target")]
    public Image backgroundImage;

    [Header("Idle backgrounds (one per level)")]
    public Sprite[] idleSprites; // Size 5

    [Header("Hover backgrounds (one per level)")]
    public Sprite[] hoverSprites; // Size 5

    private int lastHoveredIndex = -1;

    public void SetHoverBackground(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= hoverSprites.Length) return;
        backgroundImage.sprite = hoverSprites[levelIndex];
        lastHoveredIndex = levelIndex;
    }

    public void SetIdleBackground()
    {
        if (lastHoveredIndex < 0 || lastHoveredIndex >= idleSprites.Length) return;
        backgroundImage.sprite = idleSprites[lastHoveredIndex];
    }
}
