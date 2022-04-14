using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefManager : MonoBehaviour
{

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        //change character in playerprefs if it doesnt exist
        if (PlayerPrefs.GetString("character") == "")
        {
            PlayerPrefs.SetString("character", "pinguin");
        }
    }

    //change character
    public void ChangeCharacter(string newCharacter)
    {
        PlayerPrefs.SetString("character", newCharacter);
    }
}
