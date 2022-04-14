using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakesPlace : MonoBehaviour
{
    //used to determine type and how many steps back or forth
    public bool isLadder;
    public bool isSnake;
    public int steps;

    public string word;
    public Image wordImage;

    public bool noImage = false;

    void Start()
    {
        
    }

    public void SetImage()
    {
        if (!noImage)
        {
            //I know this shoudlve been set beforehand in editor but thats extra work now
            wordImage = this.transform.GetChild(0).GetComponent<Image>();

            wordImage.sprite = Resources.Load<Sprite>("Image/" + word);
            wordImage.preserveAspect = true;
        }
        this.GetComponent<WordObject>().word[0] = this.word;
    }
}
