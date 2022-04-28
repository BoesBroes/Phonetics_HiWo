using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    //for buying character and changing it and setting values

    public int cost;
    public string characterName;
    public Text nameAndCost;

    public Image characterImage;

    public bool locked;

    public OptionsManager optionsManager;

    public AudioClip bought;
    public AudioClip notEnough;
    public AudioClip switchChar;

    // Starts after optionmanager lockchecked each option
    public void StartUnlockCheck()
    {
        if (!locked)
        {
            characterImage.color = Color.white;
            nameAndCost.text = characterName;
        }
        else
        {
            characterImage.color = Color.black;
            nameAndCost.text = characterName + " kost " + cost + " punten";
        }
    }

    public void UnlockCharacter()
    {
        if (PlayerPrefs.GetInt(characterName) == 0)
        {
            if (PlayerPrefs.GetInt("points") >= cost)
            {
                int temp = PlayerPrefs.GetInt("points");
                temp -= cost;
                PlayerPrefs.SetInt("points", temp);
                PlayerPrefs.SetInt(characterName, 1);

                locked = false;
                StartUnlockCheck();

                optionsManager.ChangeCharacter(characterName);
                optionsManager.ChangePoints();

                TaskGameMode.gameMode.PlaySound(bought);
            }
            else
            {
                TaskGameMode.gameMode.PlaySound(notEnough);

            }
        }
        //change the avatar
        else
        {
            optionsManager.ChangeCharacter(characterName);
            TaskGameMode.gameMode.PlaySound(switchChar);
        }
    }
}
