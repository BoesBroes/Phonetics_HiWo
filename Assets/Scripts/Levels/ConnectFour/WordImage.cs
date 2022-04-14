using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordImage : MonoBehaviour
{


    public void FindImage(string word)
    {

        //dont forget to set new images as sprites in editor!
        if (Resources.Load<Sprite>("Image/" + word))
        {
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/" + word);
        }
        else
        {
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/kaas");
        }

        this.GetComponent<Image>().SetNativeSize();

    }
}

