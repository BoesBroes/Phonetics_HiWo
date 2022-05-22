using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLevel : MonoBehaviour
{

    public void ChangeScene(string scene)
    {
        LevelManager.levelManager.ChangeLevel(scene);
    }
}

