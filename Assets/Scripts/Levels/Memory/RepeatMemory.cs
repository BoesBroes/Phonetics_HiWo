using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepeatMemory : MonoBehaviour
{
    public Image wordImage;
    public WordObject wordObject;

    public void StartWordPlayer(string currentWord, Image image)
    {
        wordImage.gameObject.SetActive(true);
        wordImage.sprite = image.sprite;

        wordImage.preserveAspect = true;

        wordObject.word[0] = currentWord;

        RecognitionManager.recognitionManager.RecognizeWord(wordObject.gameObject, true);
    }

    public void StartWordAI(string currentWord, Image image)
    {
        wordImage.gameObject.SetActive(true);
        wordImage.sprite = image.sprite;

        wordImage.preserveAspect = true;

        wordObject.word[0] = currentWord;

        RecognitionManager.recognitionManager.RecognizeWord(wordObject.gameObject, false);
    }

    public void DisableImage()
    {
        wordImage.gameObject.SetActive(false);
    }
}
