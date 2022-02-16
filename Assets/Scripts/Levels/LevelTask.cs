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

    public string rightInput;

    void Update()
    {

    }



    public void EndTask()
    {
        //do stuff
    }

    public virtual void StartTask()
    {
    }

}
