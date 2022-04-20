using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionsManager : MonoBehaviour
{
    //decide if option is bought here

    public Option[] options;

    public Hint hint;

    public Text points;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < options.Length; i++)
        {
            if (PlayerPrefs.GetInt(options[i].characterName) == 0)
            {
                if(options[i].characterName == "pinguin")
                {
                    options[i].locked = false;
                }
                else
                {
                    options[i].locked = true;
                }
            }
            else
            {
                options[i].locked = false;
            }
            options[i].StartUnlockCheck();
        }

        points.text = "je hebt " + PlayerPrefs.GetInt("points") + " punten";
    }

    public void ChangeCharacter(string character)
    {
        PlayerPrefs.SetString("character", character);

        if (Resources.Load<Sprite>("Image/" + character))
        {
            hint.thisImage.sprite = Resources.Load<Sprite>("Image/" + character);
        }
        else
        {
            hint.thisImage.sprite = Resources.Load<Sprite>("Image/kaas");
        }
    }

    public void ChangePoints()
    {
        points.text = "je hebt " + PlayerPrefs.GetInt("points") + " punten";
    }

}
