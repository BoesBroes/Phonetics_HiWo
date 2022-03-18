using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTask : TaskMain
{

    public AudioClip startSound;

    public AudioSource sourceAudio;

    // Start is called before the first frame update
    public override void StartTask()
    {
        sourceAudio.clip = startSound;
        sourceAudio.Play();
        StartCoroutine(WaitSoundFinished());
    }

    IEnumerator WaitSoundFinished()
    {
        yield return new WaitForSeconds(startSound.length + 1.5f);
        gameMode.GetComponent<TaskGameMode>().TaskFinished();
    }
}
