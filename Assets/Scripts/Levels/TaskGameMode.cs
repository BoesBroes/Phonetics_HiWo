//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskGameMode : LevelTask
{
    //[Header("UI")]
    //public UIManager UI;

    public GameObject beginScreen;
    public GameObject endScreen;
    //public GameObject buttons;

    [Header("Tasks")]
    public GameObject taskParent;
    public LevelTask currentTask;
    private List<GameObject> tasks = new List<GameObject>();
    private int taskIndex = 0;//When a task gets done, this gets incremented
    private bool levelFinished = false;
    [HideInInspector]
    public bool tutorialTurn = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip startSceneSound;
    private bool startSoundPlayed = false;

    //for future hints
    //[Header("Hint")]
    //public Hint hint;

    public GameObject gameOverPanel;

    private bool gameOver = false;

    public Text endText;

    void Start()
    {
        PlaySound(startSceneSound);


        tasks.Clear();

        LevelTask[] allChildren = taskParent.GetComponentsInChildren<LevelTask>();
        foreach (LevelTask child in allChildren)
        {
            tasks.Add(child.gameObject);
            child.gameMode = this;
            child.gameObject.SetActive(false);
        }
        currentTask = tasks[0].GetComponent<LevelTask>();

        if (beginScreen)
        {
            beginScreen.SetActive(true);
        }

    }

    private void Update()
    {
        //start the game if the start sound has been played and the sound stopped playing
        if (!startSoundPlayed && !audioSource.isPlaying)
        {
            startSoundPlayed = true;
            if (beginScreen)
            {
                beginScreen.SetActive(false);
            }

        }
        if (levelFinished && startSoundPlayed)
        {
            if (!audioSource.isPlaying)
            {
                LevelManager.levelManager.LoadLastLevel();
            }
        }

        if(!gameOver)
        {
            currentTask.gameObject.SetActive(true);
            currentTask.StartTask();
        }    
    }

    public void StartGame()
    {
        //MusicManager.musicManager.StopMusic();

        foreach (GameObject task in tasks)
        {
            task.GetComponent<LevelTask>().gameMode = this;
        }

        //If there is no tasks set in the game mode, there won't be a task at index 0 either. so end the level so the user doesn't get stuck and report an error
        if (tasks[0])
        {
            tasks[0].SetActive(true);

        }
        else if (!tasks[0])
        {
            Debug.LogError("There is no tasks set in the semantic relationships gamemode!");
            EndLevel();
            //give the player a reward

        }

        // if (hint)
        //{
        //   hint.gameObject.SetActive(true);
        //}
    }


    public void TaskFinished(float angerChange, float happinessChange, float socialChange)
    {
        GoToNextTask();
    }

    private void ChangeColor(Image sliderFill, float value, bool anger)
    {
        if (anger)
        {
            Color statsColor = new Color(value, 1 -value, 0, 1);
            sliderFill.color = statsColor;
        }
        else
        {
            Color statsColor = new Color(1 - value, value, 0, 1);
            sliderFill.color = statsColor;
        }

    }

    private void GoToNextTask()
    {
        //only need taskindex, else object references dont work
        //disable the previous task
        if (taskIndex + 1 < tasks.Count) //if same panel is used check if its active, if active dont deactivate and activate (breaks sound)
        {
            if (tasks[taskIndex] != tasks[taskIndex + 1] && tasks[taskIndex].activeSelf)
            {
                tasks[taskIndex].SetActive(false);
            }
        }

        //increment task index
        taskIndex++;
        //enable next task if it exists
        if (taskIndex < tasks.Count)
        {
            if (!tasks[taskIndex].activeSelf)
            {
                tasks[taskIndex].SetActive(true);
                tasks[taskIndex].gameObject.GetComponent<LevelTask>().StartTask();
                currentTask = tasks[taskIndex].GetComponent<LevelTask>();
            }
        }

        //If the last task, then the level is done
        else if (taskIndex >= tasks.Count)
        {
            EndLevel();
        }
    }

    public void CheckSpeechInput(string input)
    {
        if(input.Contains(currentTask.rightInput))
        {
            GoToNextTask();
        }
    }

    private void EndLevel()
    {

        foreach (GameObject task in tasks)
        {
            task.SetActive(false);
        }


        //UI Stuff
        if (!endScreen)
        {
            Debug.LogError("No endscreen set in Gamemode. Won't be displayed!");
            return;
        }

        //if (!UI)
        //{
        //    Debug.LogError("UI Manager is not set in Gamemode and cant display the endscreen!");
        //    //LevelManager.levelManager.LoadLastLevel();
        //    return;
        //}

        //else if (UI && endScreen)
        //{
        //    //UI.OpenCanvas(endScreen.GetComponent<Canvas>());
        //    //LevelManager.levelManager.LoadLastLevel();

        //}
    }

    public void PlaySound(AudioClip audio)
    {
        audioSource.Stop();
        audioSource.clip = audio;
        audioSource.Play();
    }
}
