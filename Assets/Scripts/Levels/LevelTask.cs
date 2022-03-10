using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTask : TaskMain
{
    [HideInInspector]
    //public Hint hint;


    public AudioClip startSound;
    public AudioClip beep;
    public AudioSource sourceAudio;



    private bool runningTask = false;

    public bool wrongTask = false;

    public override void StartTask()
    {
        //add a check for future tasks to check if model has been loaded here
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

        Debug.Log("Startrecording");
        VoskSpeechToText.voskSpeechToText.VoiceProcessor.StartRecording();

        yield return new WaitForSeconds(5f);

        Debug.Log("Stoprecording");
        VoskSpeechToText.voskSpeechToText.VoiceProcessor.StopRecording();

        if(!wrongTask)
        {
            runningTask = false;
        }
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
