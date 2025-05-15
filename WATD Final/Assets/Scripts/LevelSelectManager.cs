using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for below
using DG.Tweening;

public class LevelSelectManager : MonoBehaviour
{
    public Button[] levelButtons;
    public bool[] unlockedLevels; // Must match size of levelButtons
    public Image backgroundImage; // Your level select panel's Image component

    [Header("Idle Backgrounds (5 total)")]
    public Sprite[] idleSprites;

    [Header("Hover Backgrounds (5 total)")]
    public Sprite[] hoverSprites;

    public Image backgroundBottom;
    public Image backgroundTop;
    public CanvasGroup topCanvasGroup;

    private int lastHoveredIndex = -1;

    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i;

            // On Click
            levelButtons[i].onClick.AddListener(() => OnLevelButtonClick(index));

            // Add hover events using EventTrigger
            EventTrigger trigger = levelButtons[i].gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = levelButtons[i].gameObject.AddComponent<EventTrigger>();
            }

            // Pointer Enter to Set hover background
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => OnHoverEnter(index));
            trigger.triggers.Add(enterEntry);

            // Pointer Exit to Restore idle background
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => OnHoverExit());
            trigger.triggers.Add(exitEntry);
        }
    }

    void OnLevelButtonClick(int index)
    {
        if (unlockedLevels[index])
        {
            AudioManager.instance.PlaySFX(0);
            LevelLoader.Instance.LoadThatLevel(index + 1); // Assuming level 1 = index 0
        }
        else
        {
            Debug.Log("Level " + (index + 1) + " is locked.");
        }
    }

    void OnHoverEnter(int index)
    {
        if (hoverSprites == null || index >= hoverSprites.Length)
            return;

        if (index != lastHoveredIndex)
        {
            backgroundTop.sprite = hoverSprites[index];
            topCanvasGroup.alpha = 0f;

            topCanvasGroup.DOFade(1f, 0.2f).OnComplete(() =>
            {
                backgroundBottom.sprite = hoverSprites[index];
                topCanvasGroup.alpha = 0f;
            });

            lastHoveredIndex = index;
        }
        else
        {
            backgroundBottom.sprite = hoverSprites[index];
        }
    }

    void OnHoverExit()
    {
        if (lastHoveredIndex < 0 || lastHoveredIndex >= idleSprites.Length)
            return;

        backgroundTop.sprite = idleSprites[lastHoveredIndex];
        topCanvasGroup.alpha = 0f;

        topCanvasGroup.DOFade(1f, 0.2f).OnComplete(() =>
        {
            backgroundBottom.sprite = idleSprites[lastHoveredIndex];
            topCanvasGroup.alpha = 0f;
        });
    }

}
