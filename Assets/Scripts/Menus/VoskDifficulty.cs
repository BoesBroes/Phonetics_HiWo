using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoskDifficulty : MonoBehaviour
{
    public AudioClip blip;

    public void ChangeDifficulty(int maxAlternatives)
    {
        PlayerPrefs.SetInt("difficulty", maxAlternatives);
        TaskGameMode.gameMode.PlaySound(blip);
    }
}
