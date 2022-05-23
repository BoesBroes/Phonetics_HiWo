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

    private bool voskLoading;
    public bool voskLoaded;

    public GameObject vosk;
    private bool voskExists;

    public GameObject canvas;
    public Slider slider;
    public Text loadText;
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

        DontDestroyOnLoad(this);

        voskExists = false;

        //Stops the screen on android/IOS from going to sleep
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void LoadLastLevel()
    {
        StartCoroutine(LoadLevel(lastScene));
    }

    public void ChangeLevel(string level)
    {
        //loads new level if the scene exists and isnt disabled

        //set false by default, will be enabled if vosk is active
       

        lastScene = SceneManager.GetActiveScene().name;

        //its too quick if Vosk does not have to be loaded
        if(!voskExists)
        {
            canvas.SetActive(true);
        }

        StartCoroutine(LoadLevel(level));
    }

    public IEnumerator LoadLevel(string level)
    {
        //loads new level if the scene exists and isnt disabled

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);

        asyncLoad.allowSceneActivation = false;

        loadText.text = "Level aan het laden..";

        while (!asyncLoad.isDone)
        {
            //output some progress here
            slider.value = asyncLoad.progress / 2;

            //enable depending on vosk
            if (asyncLoad.progress >= .9f)
            {

                if (voskLoaded)
                {
                    asyncLoad.allowSceneActivation = true;
                    VoskSpeechToText.voskSpeechToText.MaxAlternatives = PlayerPrefs.GetInt("difficulty");
                    canvas.SetActive(false);
                    Debug.Log("vosk loaded, scene started");
                }
                else
                {
                    if (!voskExists)
                    {
                        voskExists = true;
                        loadText.text = "Spraakherkenning aan het laden..";
                        Instantiate(vosk);
                        voskLoading = true;
                    }
                }
            }
            yield return null;
        }
    }



}
