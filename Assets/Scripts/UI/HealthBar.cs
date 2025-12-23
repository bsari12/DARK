using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient colGradient;
    private Image fillImage;

    void Awake()
    {
        fillImage = healthSlider.fillRect.GetComponent<Image>();
    }

    public void SetSliderValue(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        fillImage.color = colGradient.Evaluate(healthSlider.normalizedValue);
    }

}
