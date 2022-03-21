using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTask : TaskMain
{

    public AudioClip startSound;

    // Start is called before the first frame update
    public override void StartTask()
    {
        gameMode.PlaySound(startSound);
        StartCoroutine(WaitSoundFinished());
    }

    IEnumerator WaitSoundFinished()
    {
        yield return new WaitForSeconds(startSound.length + 1.5f);
        gameMode.GetComponent<TaskGameMode>().TaskFinished();
    }
}
