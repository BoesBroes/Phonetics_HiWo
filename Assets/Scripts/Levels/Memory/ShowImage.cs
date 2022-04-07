using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowImage : MonoBehaviour
{
    public GameObject cardImage;

    public string word;

    public Image wordImage;
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

            MemoryManager.memoryManager.ImageClicked(this);
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

        MemoryManager.memoryManager.ImageClickedAI(this);
    }

    public void DisableImage()
    {
        cardImage.SetActive(false);
    }
}
