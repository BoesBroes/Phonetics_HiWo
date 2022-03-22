using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMain : MonoBehaviour
{
    public TaskGameMode gameMode;

    public RecognitionManager recognitionManager;

    //for gamemode to find task which fall under this thing here
    virtual public void StartTask()
    {

    }

    //move on after word (overriden in task that inherits this main Task)
    virtual public void Proceed()
    {

    }
}
