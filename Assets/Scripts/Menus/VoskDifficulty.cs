using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoskDifficulty : MonoBehaviour
{
    public AudioClip blip;
    public Image[] buttonImage;

    private void OnEnable()
    {
        switch (PlayerPrefs.GetInt("difficulty"))
        {
            case 3:
                buttonImage[0].color = Color.green;
                break;
            case 5:
                buttonImage[1].color = Color.green;
                break;
            case 7:
                buttonImage[2].color = Color.green;
                break;
        }
    }

    public void ChangeDifficulty(int maxAlternatives)
    {
        PlayerPrefs.SetInt("difficulty", maxAlternatives);
        TaskGameMode.gameMode.PlaySound(blip);

        for(int i = 0; i < buttonImage.Length; i++)
        {
            buttonImage[i].color = Color.white;
        }

        switch (maxAlternatives)
        {
            case 3:
                buttonImage[0].color = Color.green;
                break;
            case 5:
                buttonImage[1].color = Color.green;
                break;
            case 7:
                buttonImage[2].color = Color.green;
                break;

        }
    }
}
