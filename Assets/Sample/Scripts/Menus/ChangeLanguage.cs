using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLanguage : MonoBehaviour
{
    //public string english;
    //public string dutch;

    public GameObject languageController; //thecontroller

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("languages") == null)
        {
            PlayerPrefs.SetString("languages", "vosk-model-nl-spraakherkenning-0.6/");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLanguageSpeech(string newLanguage)
    {
        PlayerPrefs.SetString("languages", newLanguage);
    }
}
