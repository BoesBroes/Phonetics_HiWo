using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    [HideInInspector]
    //public Hint hint;

    public TaskGameMode gameMode;

    public AudioClip startSound;


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
