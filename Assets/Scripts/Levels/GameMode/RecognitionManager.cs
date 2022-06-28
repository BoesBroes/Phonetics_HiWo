using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecognitionManager : MonoBehaviour
{
    public TaskGameMode gameMode;

    public static RecognitionManager recognitionManager;

    //sounds for hint
    public AudioClip startSound;
    public AudioClip que;
    public AudioClip tryAgain;

    public AudioClip recognizeSound;
    public AudioClip jingle;

    public AudioClip noDetection;
    public AudioClip notRecognized;

    //private bool runningRecognition = false;
    private bool recognized = false;

    //used to check determine how many results may be recognized (is set from voskresulttext)
    public int attemptsLength;

    public int maxAttempts = 2;
    public int totalAttempts = 0;
    public int currentAttempts = 0;

    private GameObject currentWord;

    public bool firstWordCheck = true;
    public bool attempt = false;

    public bool continueChecks = true;
    void Awake()
    {
        if (recognitionManager == null)
        {
            recognitionManager = this;
        }
        else
        {
            Destroy(recognitionManager);
            recognitionManager = this;
        }
    }

    public void RecognizeWord(GameObject currentWordObject, bool player)
    {
        recognized = false;

        currentWord = currentWordObject;
        if (player)
        {
            StartCoroutine(WaitForSound(startSound));
        }
        else
        {
            StartCoroutine(AITurnSound());
        }
    }

    IEnumerator WaitForSound(AudioClip currentClip)
    {
        gameMode.PlaySound(currentClip);

        yield return new WaitForSeconds(currentClip.length);

        firstWordCheck = true;

        if (Resources.Load<AudioClip>("WordSound/" + currentWord.GetComponent<WordObject>().word[0]))
        {
            AudioClip temp = Resources.Load<AudioClip>("WordSound/" + currentWord.GetComponent<WordObject>().word[0]);
            gameMode.PlaySound(temp);
            yield return new WaitForSeconds(temp.length);
        }
        else
        {
            AudioClip temp = Resources.Load<AudioClip>("WordSound/" + "NoSound");
            gameMode.PlaySound(temp);
            yield return new WaitForSeconds(temp.length);
            Debug.Log("no sound could be found");
        }

        //cue removed because testing showed not necessary
        //gameMode.PlaySound(que);
        //yield return new WaitForSeconds(que.length);

        continueChecks = true;

        //Debug.Log("Startrecording");
        VoskSpeechToText.voskSpeechToText.VoiceProcessor.StartRecording();

        yield return new WaitForSeconds(5f);

        //Debug.Log("Stoprecording");
        VoskSpeechToText.voskSpeechToText.VoiceProcessor.StopRecording();

        //runningRecognition = false;
    }

    public void RunCheck(string result)
    {
        if (recognized == false)
        {
            if(!attempt)
            {
                attempt = true;
                totalAttempts++;
            }    

            WordObject wordClass = currentWord.GetComponent<WordObject>();
            for (int i = 0; i < wordClass.word.Length; i++)
            {
                if (result.Contains(wordClass.word[i]))
                {
                    recognized = true;

                    if (totalAttempts == 1)
                    {
                        gameMode.attempts++;
                        gameMode.rightAttempts++;
                    }
                    else
                    {
                        gameMode.attempts++;
                        gameMode.rightAttempts += .75f;
                    }

                    totalAttempts = 0;
                    currentAttempts = 0;

                    attempt = false;

                    StartCoroutine(Recognized());               

                    return;
                }
            }

            //if no input (dont remember why it always 'unks')
            if(result == "" && firstWordCheck || result == "<unk>" && firstWordCheck)
            {
                continueChecks = false;
                firstWordCheck = false;
                StartCoroutine(WaitForSound(noDetection));
                return;
            }

            if (continueChecks)
            {
                //if word not recognized
                currentAttempts++;

                if (totalAttempts == maxAttempts && currentAttempts == attemptsLength)
                {
                    //Debug.Log("lol");
                    attempt = false;
                    firstWordCheck = true;
                    totalAttempts = 0;
                    currentAttempts = 0;
                    gameMode.attempts++;
                    StartCoroutine(NotRecognized());
                }
                else if (currentAttempts == attemptsLength)
                {
                    //Debug.Log("lel");
                    attempt = false;
                    firstWordCheck = true;
                    currentAttempts = 0;
                    StartCoroutine(WaitForSound(tryAgain));
                }
            }
        }
        else
        {
            return;
        }
    }

    IEnumerator AITurnSound()
    {
        if (Resources.Load<AudioClip>("WordSound/" + currentWord.GetComponent<WordObject>().word[0]))
        {
            AudioClip temp = Resources.Load<AudioClip>("WordSound/" + currentWord.GetComponent<WordObject>().word[0]);
            gameMode.PlaySound(temp);
            yield return new WaitForSeconds(temp.length +2f);
        }
        else
        {
            Debug.Log("no sound could be found");
            AudioClip temp = Resources.Load<AudioClip>("WordSound/" + "NoSound");
            gameMode.PlaySound(temp);
            yield return new WaitForSeconds(temp.length);
        }
        EndRecognition();
    }


    IEnumerator Recognized()
    {
        gameMode.PlaySound(recognizeSound);
        yield return new WaitForSeconds(recognizeSound.length);

        gameMode.PlaySound(jingle);
        yield return new WaitForSeconds(jingle.length);

        //continue to next turn
        EndRecognition();
    }

    IEnumerator NotRecognized()
    {
        gameMode.PlaySound(notRecognized);
        //extra magic number added or it would be a little too quick
        yield return new WaitForSeconds(notRecognized.length + 0.25f);

        //continue
        EndRecognition();
    }


    private void EndRecognition()
    {
        //if word recognized
        gameMode.currentTask.Proceed();
    }
}
