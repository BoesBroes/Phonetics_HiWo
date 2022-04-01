using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorWin : MonoBehaviour
{

    public Text winText;

    public string[] colors;

    public TaskGameMode gameMode;

    public AudioClip partOne;
    public AudioClip partTwo;

    public void ChangeText(int colorint)
    {
        winText.text = "De kleur " + colors[colorint] + " heeft gewonnen!";

        StartCoroutine(WinSounds(colorint));
    }

    IEnumerator WinSounds(int colorint)
    {
        gameMode.PlaySound(partOne);
        yield return new WaitForSeconds(partOne.length);

        AudioClip temp = (Resources.Load<AudioClip>("WordSound/" + colors[colorint]));

        gameMode.PlaySound(temp);
        yield return new WaitForSeconds(temp.length);

        gameMode.PlaySound(partOne);
        yield return new WaitForSeconds(partTwo.length);
    }
}
