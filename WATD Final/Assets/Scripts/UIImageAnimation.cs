using UnityEngine;
using UnityEngine.UI;

public class UIImageAnimation : MonoBehaviour
{
    public Sprite[] spriteFrames;
    public float frameRate = 0.1f;
    private Image imageComponent;
    private int currentFrame;
    private float timer;

    void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % spriteFrames.Length;
            imageComponent.sprite = spriteFrames[currentFrame];
            
        }
    }
}
