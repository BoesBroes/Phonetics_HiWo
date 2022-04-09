using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    public TaskGameMode gameMode;

    public AudioClip hint;

    private bool waitPlay;

    public GameObject[] objectHighlight;
    private Vector2[] currentVector;

    public float sizeIncrease;

    public float speed;

    public float waitMaxSizeTime = 2;

    private Vector2 currentIncrease;

    private float timeElapsed;

    private Color[] color;

    private Image[] imageColor;

    void Start()
    {
        currentVector = new Vector2[objectHighlight.Length];
        color = new Color[objectHighlight.Length];
        imageColor = new Image[objectHighlight.Length];

        for (int i = 0; i < objectHighlight.Length; i++)
        {
            currentVector[i] = objectHighlight[i].transform.localScale;
            color[i] =  objectHighlight[i].GetComponent<Image>().color;
            imageColor[i] = objectHighlight[i].GetComponent<Image>();
        }
    }

    public void GiveHint()
    {
        if (!gameMode.audioSource.isPlaying && waitPlay == false)
        {
            waitPlay = true;

            gameMode.PlaySound(hint);

            if(objectHighlight[0].activeSelf)
            {
                StartCoroutine(HighLightObjects());
            }
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
                objectHighlight[i].transform.localScale = Vector2.Lerp(currentVector[i], new Vector2(currentVector[i].x * sizeIncrease, currentVector[i].y * sizeIncrease), timeElapsed / speed);
                imageColor[i].color = Color.Lerp(color[i], Color.yellow, timeElapsed / speed);
            }

            currentIncrease.x = objectHighlight[0].transform.localScale.x / currentVector[0].x;
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        StartCoroutine(Dehighlight());
    }

    IEnumerator Dehighlight()
    {
        yield return new WaitForSeconds(waitMaxSizeTime);

        timeElapsed = 0;

        while (currentVector[0].x < objectHighlight[0].transform.localScale.x)
        {
            for (int i = 0; i < objectHighlight.Length; i++)
            {
                objectHighlight[i].transform.localScale = Vector2.Lerp(new Vector2(currentVector[i].x * sizeIncrease, currentVector[i].y * sizeIncrease), currentVector[i], timeElapsed / speed);
                imageColor[i].color = Color.Lerp(Color.yellow, color[i], timeElapsed / speed);
            }

            currentIncrease.x = objectHighlight[0].transform.localScale.x / currentVector[0].x;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        waitPlay = false;
    }
}
