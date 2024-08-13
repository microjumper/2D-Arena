using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        slider.minValue = 0;
        slider.maxValue = 0;
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerHeathChange += UpdateView;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerHeathChange -= UpdateView;   
    }

    private void UpdateView(int value)
    {
        if(slider.maxValue == 0)
        {
            slider.maxValue = value;
        }

        slider.value = value;
    }

}
