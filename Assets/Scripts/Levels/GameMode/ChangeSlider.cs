using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeSlider : MonoBehaviour
{
    public Slider slider;
    public Image sliderImage;

    public Text coinsText;

    public TaskGameMode gameMode;
    public AudioClip clip;
    public AudioClip coinClip;

    public float slideSpeed = 1f;

    private float timeElapsed;

    private float coinFloat = 0;

    public Text grade;
    //but also play sound

    public GameObject[] medals;
    public AudioClip medalSound;

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

            timeElapsed += Time.fixedDeltaTime;

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

        //Color lastStatsColor = new Color(1 - slider.value, slider.value, 0, 1);

        if (slider.value == 1)
        {
            gameMode.PlaySound(coinClip);
        }

        yield return new WaitForSeconds(coinClip.length);

        gameMode.PlaySound(clip);

        int temp = PlayerPrefs.GetInt("points");
        int tempIncrease = 20;
        for(float i = 0.2f; i <= slider.value; i += 0.2f)
        {
            temp += tempIncrease;
            tempIncrease += 20;
        }
        coinsText.text = "Je hebt +" + (temp - PlayerPrefs.GetInt("points")) + " punten gewonnen";

        grade.text = "Cijfer: " + (Mathf.Round(slider.value * 1000) / 100);

        PlayerPrefs.SetInt("points", temp);

        StartCoroutine(ShowMedals());
    }

    IEnumerator ShowMedals()
    {
        if(slider.value >= .5)
        {
            medals[0].SetActive(true);
            gameMode.PlaySound(medalSound);
            yield return new WaitForSeconds(medalSound.length / 2);
        }
        else
        {
            yield return null;
        }

        if (slider.value >= .65)
        {
            medals[1].SetActive(true);
            gameMode.PlaySound(medalSound);
            yield return new WaitForSeconds(medalSound.length / 2);
        }

        if (slider.value >= .8)
        {
            medals[2].SetActive(true);
            gameMode.PlaySound(medalSound);
            yield return new WaitForSeconds(medalSound.length / 2);
        }

        if (slider.value >= .9)
        {
            medals[3].SetActive(true);
            gameMode.PlaySound(medalSound);
            yield return new WaitForSeconds(medalSound.length / 2);
        }
    }
}
