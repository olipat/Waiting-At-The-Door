using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject sliderObject;

    public void Update()
    {
        if (GameManager.instance.FightingBoss == true)
        {
            sliderObject.gameObject.SetActive(true);
        }
        else
        {
            sliderObject.gameObject.SetActive(false);
        }

        
    }

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
