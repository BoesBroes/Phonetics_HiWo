using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class VoskResultText : MonoBehaviour 
{
    public VoskSpeechToText VoskSpeechToText;
    public Text ResultText;

    public GameObject taskGameMode;

    void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
    }

    private void OnTranscriptionResult(string obj)
    {
        Debug.Log(obj);
        ResultText.text = "Recognized: ";
        var result = new RecognitionResult(obj);
        for (int i = 0; i < result.Phrases.Length; i++)
        {
            if (i > 0)
            {
                ResultText.text += "\n ---------- \n";
            }

            ResultText.text += result.Phrases[0].Text + " | " + "Confidence: " + result.Phrases[0].Confidence;
        }
        if(taskGameMode)
        {
            taskGameMode.GetComponent<TaskGameMode>().CheckSpeechInput(result.Phrases[0].Text);
        }
    }
}
