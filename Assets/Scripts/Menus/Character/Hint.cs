using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    public Image thisImage;

    public TaskGameMode gameMode;

    public AudioClip hintClip;
    private bool waitPlay;

    public GameObject[] objectHighlight;
    private Vector2[] currentVector;

    public float sizeIncrease;
    public float speed;
    public float waitMaxSizeTime = 2; //time it stays at increased size
    private Vector2 currentIncrease;

    private float timeElapsed;

    private Color[] color;
    private Image[] imageColor;

    public float timeInactive = 30f;
    private float currentTime;

    public bool hintOnStart = false;

    private bool startRan = false;
    //private bool hintRunning = false; //used in case user interacts while hint is running to reset to original state
    //private bool resetHighlightsOnTurn = false;
    void Start()
    {
        if(PlayerPrefs.GetString("character") != "")
        {
            thisImage.sprite = Resources.Load<Sprite>("Image/" + PlayerPrefs.GetString("character"));
        }
        else
        {
            thisImage.sprite = Resources.Load<Sprite>("Image/pinguin");
        }
        thisImage.preserveAspect = true;

        currentVector = new Vector2[objectHighlight.Length];
        color = new Color[objectHighlight.Length];
        imageColor = new Image[objectHighlight.Length];

        for (int i = 0; i < objectHighlight.Length; i++)
        {
            currentVector[i] = objectHighlight[i].transform.localScale;
            color[i] =  objectHighlight[i].GetComponent<Image>().color;
            imageColor[i] = objectHighlight[i].GetComponent<Image>();
        }

        //if(hintOnStart)
        //{
        //    StartCoroutine(WaitForStartHint());
        //}

        startRan = true;
    }

    //seems necessary to wait a sec for memory game, less than 1.5 secs makes it look ugly
    //works on dev pc but might depend on performance and could still have issues on lower end hardware
    //update, now used because talktask starts -1.5f earlier than voice is finished (hmm this could be done better
    //now its played after level has finished loading the board or anything it needs (still could be done better, mostly in looks but no use in cleaning it up)

    //now used for if audio still active
    public IEnumerator WaitForStartHint()
    {
        yield return new WaitForSeconds(1.75f);
        GiveHint();
    }

    //innefficient, use list next time, used in case gameobjects are destroyed
    public void ReFindColors()
    {
        for (int i = 0; i < objectHighlight.Length; i++)
        {
            currentVector[i] = objectHighlight[i].transform.localScale;
            color[i] = objectHighlight[i].GetComponent<Image>().color;
            imageColor[i] = objectHighlight[i].GetComponent<Image>();
        }
    }

    public void GiveHint()
    {
        //hintRunning = true;

        if (!gameMode.audioSource.isPlaying && waitPlay == false && !TaskGameMode.gameMode.noHints)
        {
            waitPlay = true;

            gameMode.PlaySound(hintClip);

            StartCoroutine(HighLightObjects());
        }
        else if (waitPlay == false)
        {
            waitPlay = true;
            StartCoroutine(WaitForCurrentSoundToFinish());
        }
    }

    IEnumerator WaitForCurrentSoundToFinish()
    {
        yield return new WaitForSeconds(gameMode.audioSource.clip.length);

        waitPlay = false;

        GiveHint();
    }

    IEnumerator HighLightObjects()
    {
        timeElapsed = 0;

        while(currentIncrease.x < sizeIncrease)
        {
            for(int i = 0; i < objectHighlight.Length; i++)
            {
                if (objectHighlight[i].activeSelf)
                {
                    objectHighlight[i].transform.localScale = Vector2.Lerp(currentVector[i], new Vector2(currentVector[i].x * sizeIncrease, currentVector[i].y * sizeIncrease), timeElapsed / speed);
                    imageColor[i].color = Color.Lerp(color[i], Color.yellow, timeElapsed / speed);
                }
            }

            //check the first object thats active, return if none are active
            for (int i = 0; i < objectHighlight.Length; i++)
            {
                if(objectHighlight[i].activeSelf)
                {
                    currentIncrease.x = objectHighlight[i].transform.localScale.x / currentVector[i].x;
                    break;
                }
                else if(i == objectHighlight.Length)
                {
                    yield return null;
                }
            }


            timeElapsed += Time.fixedDeltaTime;

            yield return null;
        }
        StartCoroutine(Dehighlight());
    }

    IEnumerator Dehighlight()
    {
        yield return new WaitForSeconds(waitMaxSizeTime);

        timeElapsed = 0;

        //the current size divided by the original is 1
        while (currentIncrease.x > 1)
        {
            for (int i = 0; i < objectHighlight.Length; i++)
            {
                if (objectHighlight[i].activeSelf)
                {
                    objectHighlight[i].transform.localScale = Vector2.Lerp(new Vector2(currentVector[i].x * sizeIncrease, currentVector[i].y * sizeIncrease), currentVector[i], timeElapsed / speed);
                    imageColor[i].color = Color.Lerp(Color.yellow, color[i], timeElapsed / speed);
                }
            }

            //check the first object thats active, return if none are active
            for (int i = 0; i < objectHighlight.Length; i++)
            {
                if (objectHighlight[i].activeSelf)
                {
                    currentIncrease.x = objectHighlight[i].transform.localScale.x / currentVector[i].x;
                    break;
                }
                else if (i == objectHighlight.Length)
                {
                    yield return null;
                }
            }


            timeElapsed += Time.fixedDeltaTime;

            yield return null;
        }

        //as long as they return to normal (max size doesnt necesarrily need this)
        for (int i = 0; i < objectHighlight.Length; i++)
        {
            objectHighlight[i].transform.localScale = currentVector[i];
        }

        waitPlay = false;
        //hintRunning = false;
    }

    private void Update()
    {
        if(!TaskGameMode.gameMode.noHints)
        {
            currentTime += Time.deltaTime;
            //if(resetHighlightsOnTurn)
            //{
            //    resetHighlightsOnTurn = false;
            //}
        }
        else
        {
            currentTime = 0f;

            //if hint was running and user interacted
            //if(hintRunning)
            //{
            //    hintRunning = false;
            //    resetHighlightsOnTurn = true;
            //}
        }
        if (currentTime >= timeInactive)
        {
            currentTime = Time.deltaTime;
            //Debug.Log("hint played");
            GiveHint();
        }
        if (Input.anyKey)
        {
            //reset timer, no hint played 
            currentTime = Time.deltaTime;
            //Debug.Log("hint avoided");
        }
    }

    //normal people put onenable at the top of the script... I think?
    private void OnEnable()
    {
        //dont run the first time (before some stuff initialized (not needed anyway, but a check to reset all objects in case a user interacted while highlighted))
        if (startRan)
        {
            for (int i = 0; i < objectHighlight.Length; i++)
            {
                if (objectHighlight[i].activeSelf)
                {
                    objectHighlight[i].transform.localScale = currentVector[i];
                    imageColor[i].color = color[i];
                }
            }
            waitPlay = false;
        }
    }
}
