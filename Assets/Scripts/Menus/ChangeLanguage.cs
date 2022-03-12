using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLanguage : MonoBehaviour
{
    void Start()
    {
        //change language in playerprefs if it doesnt exist
        //language gets loaded in VoskSpeechToText at start of level
        if (PlayerPrefs.GetString("languages") != "vosk-model-nl-spraakherkenning-0.6.zip")
        {
            PlayerPrefs.SetString("languages", "vosk-model-nl-spraakherkenning-0.6.zip");
        }
    }

    //change language
    public void ChangeLanguageSpeech(string newLanguage)
    {
        PlayerPrefs.SetString("languages", newLanguage);
    }
}
