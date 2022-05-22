//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskGameMode : TaskMain
{
    public static TaskGameMode gameMode;

    //[Header("UI")]
    //public UIManager UI;
    public GameObject endScreenWin;
    public GameObject endScreenLose;
    public GameObject endScreenStale;

    //really its more efficient to send this information directly to a function but time
    public bool win;
    public bool lose;
    public bool stale;

    //public GameObject buttons;

    [Header("Tasks")]
    public GameObject taskParent;
    public TaskMain currentTask;
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

    private int stringCheckCount;
    private bool stringCheckRunning;


    //the result text used to make clear its loading (for speech recognition test)
    public Text resultText;

    //used to calculate end results/progress bar
    public float attempts;
    public float rightAttempts;
    private float accuracy;

    //shoudlve set hint here earlier and a NoHint function instead of the current works (oh well)
    public Hint hint;

    public bool noHints;
    
    void Awake()
    {
        if (gameMode == null)
        {
            gameMode = this;
        }
        else
        {
            Destroy(gameMode);
            gameMode = this;
        }
    }

    void Start()
    {
        //startsound handled by task itself (for now)
        //PlaySound(startSceneSound);

        //clear the list and create a new list with all objects containing TaskMain under task gameobject
        tasks.Clear();

        if(taskParent != null)
        {
            TaskMain[] allChildren = taskParent.GetComponentsInChildren<TaskMain>();
            foreach (TaskMain child in allChildren)
            {
                tasks.Add(child.gameObject);
                child.gameMode = this;
                child.gameObject.SetActive(false);
            }
            currentTask = tasks[0].GetComponent<TaskMain>();
            currentTask.gameObject.SetActive(true);

            currentTask.StartTask();
        }

        
    }

    //private void Update()
    //{
    //    if(!gameOver)
    //    {
    //        //currentTask.gameObject.SetActive(true);
    //        //currentTask.StartTask();
    //    }    
    //}

    public void StartGame()
    {
        //MusicManager.musicManager.StopMusic();

        foreach (GameObject task in tasks)
        {
            task.GetComponent<TaskMain>().gameMode = this;
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


    public void TaskFinished()
    {
        currentTask.StopAllCoroutines();
        GoToNextTask();
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
                tasks[taskIndex].gameObject.GetComponent<TaskMain>().StartTask();
                currentTask = tasks[taskIndex].GetComponent<TaskMain>();
            }
        }

        //If the last task, then the level is done
        else if (taskIndex >= tasks.Count)
        {
            currentTask.gameObject.SetActive(false);
            EndLevel();
        }
    }

    public void CheckSpeechInput(string input)
    {
        //stringCheckCount++;

        //do not check if its a specific type of test for speech evaluation
        //if(currentTask.GetComponent<LevelTask>().wrongWordTask)
        //{
        //    if (input.Contains(currentTask.rightInput))
        //    {
        //        StopCoroutine(WaitForAllStrings());
        //        GoToNextTask();
        //        stringCheckCount = 0;
        //        stringCheckRunning = false;
        //    }
        //    else if (stringCheckCount == 3)
        //    {
        //        StopCoroutine(WaitForAllStrings());
        //        currentTask.GetComponent<TaskMain>().StartTask();
        //        stringCheckCount = 0;
        //        stringCheckRunning = false;
        //    }

        //    if (!stringCheckRunning)
        //    {
        //        stringCheckRunning = true;
        //        StartCoroutine(WaitForAllStrings());
        //    }
        //}
        //else
        //{
        //    return;
        //}
    }

    IEnumerator WaitForAllStrings()
    {
        var thisTask = currentTask;
        yield return new WaitForSeconds(5f);
        if (stringCheckCount != 3 && stringCheckRunning && thisTask == currentTask)
        {
            stringCheckCount = 0;
            stringCheckRunning = false;
            currentTask.GetComponent<TaskMain>().StartTask();
        }
    }

    public void EndLevel()
    {
        accuracy = rightAttempts / attempts;

        foreach (GameObject task in tasks)
        {
            task.SetActive(false);
        }

        hint.gameObject.SetActive(false);

        if(win)
        {
            endScreenWin.SetActive(true);
            endScreenWin.GetComponent<ChangeSlider>().SliderChange(accuracy);
        }
        else if(lose)
        {
            endScreenLose.SetActive(true);
            endScreenLose.GetComponent<ChangeSlider>().SliderChange(accuracy);
        }
        else if(stale)
        {
            endScreenStale.SetActive(true);
            endScreenStale.GetComponent<ChangeSlider>().SliderChange(accuracy);
        }
        else
        {
            Debug.LogError("no end condition is defined?!");
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

    //simple function to play any sound
    public void PlaySound(AudioClip audio)
    {
        audioSource.Stop();
        audioSource.clip = audio;
        audioSource.Play();
    }

    //shouldve done this earlier
    public void StartMultipleSounds(AudioClip[] audio)
    {
        StartCoroutine(PlayMultipleSounds(audio));
    }

    IEnumerator PlayMultipleSounds(AudioClip[] audio)
    {
        for(int i = 0; i < audio.Length; i++)
        {
            audioSource.Stop();
            audioSource.clip = audio[i];
            audioSource.Play();
            yield return new WaitForSeconds(audio[i].length);
        }
    }
}
