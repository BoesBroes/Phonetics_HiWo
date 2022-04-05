using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeSlider : MonoBehaviour
{
    public Slider slider;
    public Image sliderImage;
    public void SliderChange(float accuracy)
    {
        slider.value = accuracy;

        Color statsColor = new Color(1 - accuracy, accuracy, 0, 1);

        sliderImage.color = statsColor;
    }
}
