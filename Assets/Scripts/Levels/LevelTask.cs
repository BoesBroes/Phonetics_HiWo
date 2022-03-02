using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTask : MonoBehaviour
{
    [HideInInspector]
    //public Hint hint;

    public TaskGameMode gameMode;

    public AudioClip startSound;
    public AudioClip beep;
    public AudioSource sourceAudio;

    public string rightInput;


    private bool runningTask = false; 

    public void StartTask()
    {
        if (!runningTask)
        {
            runningTask = true;
            sourceAudio.clip = startSound;
            sourceAudio.Play();
            StartCoroutine(WaitSoundFinished());
        }
        else
        {
           // StartCoroutine(WaitCoroutine());
        }
    }

    IEnumerator WaitSoundFinished()
    {
        yield return new WaitForSeconds(startSound.length + .5f);
        sourceAudio.clip = beep;
        sourceAudio.Play();


        yield return new WaitForSeconds(beep.length);

        VoskSpeechToText.voskSpeechToText.VoiceProcessor.StartRecording();

        yield return new WaitForSeconds(5f);

        VoskSpeechToText.voskSpeechToText.VoiceProcessor.StopRecording();

        runningTask = false;
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(5f);

        //VoskSpeechToText.voskSpeechToText.VoiceProcessor.StopRecording();

        runningTask = false;

        StartTask();
    }

    public void EndTask()
    {
        //do stuff
    }

}
