using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;

    public string[] scenes;

    private string lastScene;

    //ben ik code vergeten te saven?:'^)

    // Start is called before the first frame update
    void Awake()
    {
        if (levelManager == null)
        {
            levelManager = this;
        }
        else
        {
            Destroy(gameObject);
        }


        //Stops the screen on android/IOS from going to sleep
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void LoadLastLevel()
    {
        ChangeLevel(lastScene);
    }

    public void ChangeLevel(string level)
    {
        //loads new level if the scene exists and isnt disabled

        lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(level);


        //else
        //{
        //    Debug.LogError("Scene either does not exist or is disabled. Do you have the correct path?");
        //}
    }



}
