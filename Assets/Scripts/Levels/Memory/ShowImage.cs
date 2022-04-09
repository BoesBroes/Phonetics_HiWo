using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowImage : MonoBehaviour
{
    public GameObject cardImage;

    public string word;

    public Image wordImage;

    public AudioClip cardTurn;

    public void CheckImage()
    {

        //add if 2 images are shown 
        if (MemoryManager.memoryManager.clicks < 2 && MemoryManager.memoryManager.playerTurn)
        {
            if(!wordImage.sprite)
            {
                if (Resources.Load<Sprite>("Image/" + word))
                {
                    wordImage.sprite = Resources.Load<Sprite>("Image/" + word);
                }
                else
                {
                    wordImage.sprite = Resources.Load<Sprite>("Image/kaas");
                }

                wordImage.preserveAspect = true;
            }

            cardImage.SetActive(true);

            StartCoroutine(WaitForSound(false));
        }
    }

    public void AICheckImage()
    {
        if (!wordImage.sprite)
        {
            if (Resources.Load<Sprite>("Image/" + word))
            {
                wordImage.sprite = Resources.Load<Sprite>("Image/" + word);
            }
            else
            {
                wordImage.sprite = Resources.Load<Sprite>("Image/kaas");
            }

            wordImage.preserveAspect = true;
        }

        cardImage.SetActive(true);

        StartCoroutine(WaitForSound(true));
    }

    public void DisableImage()
    {
        cardImage.SetActive(false);
    }

    IEnumerator WaitForSound(bool AI)
    {
        MemoryManager.memoryManager.gameMode.PlaySound(cardTurn);

        yield return new WaitForSeconds(cardTurn.length);

        if(AI)
        {
            MemoryManager.memoryManager.ImageClickedAI(this);
        }
        else
        {
            MemoryManager.memoryManager.ImageClicked(this);

        }
    }
}
