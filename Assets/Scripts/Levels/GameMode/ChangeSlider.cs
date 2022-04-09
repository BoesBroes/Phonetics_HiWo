using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeSlider : MonoBehaviour
{
    public Slider slider;
    public Image sliderImage;


    public TaskGameMode gameMode;
    public AudioClip clip;
    public AudioClip coinClip;

    public float slideSpeed = 1f;

    private float timeElapsed;

    private float coinFloat = 0;
    //but also play sound
    public void SliderChange(float accuracy)
    {
        //slider.value = accuracy;

        //Color statsColor = new Color(1 - accuracy, accuracy, 0, 1);

        //sliderImage.color = statsColor;

        StartCoroutine(SmoothSlide(accuracy));

    }

    IEnumerator SmoothSlide(float accuracy)
    {
        while(slider.value < accuracy - 0.005f)
        {
            slider.value = Mathf.Lerp(slider.value, accuracy, timeElapsed / slideSpeed);

            timeElapsed += Time.deltaTime;

            Color statsColor = new Color(1 - slider.value, slider.value, 0, 1);

            sliderImage.color = statsColor;

            if(slider.value >= coinFloat + 0.2f)
            {
                coinFloat += 0.2f;
                gameMode.PlaySound(coinClip);
            }
            //everytime float =+ x play sound

            yield return null;
        }

        slider.value = accuracy;

        Color lastStatsColor = new Color(1 - slider.value, slider.value, 0, 1);

        sliderImage.color = lastStatsColor;

        if (slider.value == 1)
        {
            gameMode.PlaySound(coinClip);
            yield return new WaitForSeconds(coinClip.length);
        }

        Debug.Log("finished");

        gameMode.PlaySound(clip);
    }
}
