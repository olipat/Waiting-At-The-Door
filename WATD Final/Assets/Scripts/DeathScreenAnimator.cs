using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathScreenAnimator : MonoBehaviour
{
    public static DeathScreenAnimator Instance;

    public void Awake()
    {
        if (Instance == null)
        Instance = this;
    }

    public Image imageComponent;
    public Sprite[] animationFrames;
    public float frameRate = 0.1f;

    [Header("Trigger Animation")]
    public bool shouldPlay = false;

    private Coroutine animationRoutine;

    void Update()
    {
        if (shouldPlay && animationRoutine == null)
        {
            animationRoutine = StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        for (int i = 0; i < animationFrames.Length; i++)
        {
            imageComponent.sprite = animationFrames[i];
            yield return new WaitForSeconds(frameRate);
        }
        
        // Reset for next use
        shouldPlay = false;
        animationRoutine = null;

        Time.timeScale = 0f;
    }
}
