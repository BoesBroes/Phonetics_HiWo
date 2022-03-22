using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class VoskResultText : MonoBehaviour 
{
    public VoskSpeechToText VoskSpeechToText;
    public Text ResultText;

    //public GameObject taskGameMode;

    void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
    }

    private void OnTranscriptionResult(string obj)
    {
        Debug.Log(obj);
        ResultText.text = "Recognized: ";
        ResultText.text += "\n ---------- \n";
        var result = new RecognitionResult(obj);
        for (int i = 0; i < result.Phrases.Length; i++)
        {
            if (i > 0)
            {
                ResultText.text += "\n ---------- \n";
            }

            //simplified results for test
            //ResultText.text += result.Phrases[i].Text + " | " + "Confidence: " + result.Phrases[i].Confidence;
            ResultText.text += result.Phrases[i].Text;
        }
        if (RecognitionManager.recognitionManager)
        {
            RecognitionManager.recognitionManager.attemptsLength = result.Phrases.Length;
            for (int i = 0; i < result.Phrases.Length; i++)
            {
                RecognitionManager.recognitionManager.RunCheck(result.Phrases[i].Text);
            }
        }
    }
}
