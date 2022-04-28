using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordRepeater : MonoBehaviour
{
    public string[] words;

    public GameObject wordImage;

    public WordObject wordObject;

    public Image thisImage;
    public void StartWord()
    {
        int temp = Random.Range(0, words.Length);

        wordObject.word[0] = words[temp];

        wordImage.SetActive(true);

        //dont forget to set new images as sprites in editor!
        if (Resources.Load<Sprite>("Image/" + words[temp]))
        {
            wordImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/" + words[temp]);
        }
        else
        {
            wordImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/kaas");
        }

        wordImage.GetComponent<Image>().SetNativeSize();

        RecognitionManager.recognitionManager.RecognizeWord(wordObject.gameObject, true);
    }

    public void AIWord()
    {
        int temp = Random.Range(0, words.Length);

        wordObject.word[0] = words[temp];

        wordImage.SetActive(true);

        //Debug.Log(words[temp]);

        //dont forget to set new images as sprites in editor!
        if (Resources.Load<Sprite>("Image/" + words[temp]))
        {
            wordImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/" + words[temp]);
        }
        else
        {
            wordImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/kaas");
        }

        wordImage.GetComponent<Image>().SetNativeSize();

        RecognitionManager.recognitionManager.RecognizeWord(wordObject.gameObject, false);
    }

    public void StopWord()
    {
        wordImage.SetActive(false);
    }
}
