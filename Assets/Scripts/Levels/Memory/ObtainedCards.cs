using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObtainedCards : MonoBehaviour
{
    private Image[] thisImage;
    private int count;

    // Start is called before the first frame update
    
    void Start()
    {
        //setting component in editor would be more efficient..
        thisImage = new Image[this.transform.childCount];
        for(int i = 0; i < this.transform.childCount; i++)
        {
            thisImage[i] = this.transform.GetChild(i).GetComponent<Image>();
            thisImage[i].gameObject.SetActive(false);
        }
    }

    
    public void CardObtained(Sprite image)
    {
        thisImage[count].gameObject.SetActive(true);
        thisImage[count].sprite = image;
        thisImage[count].preserveAspect = true;
        count++;
    }
}
