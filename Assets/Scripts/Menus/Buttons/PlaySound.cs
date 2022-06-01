using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip clip;
    public void Play()
    {
        TaskGameMode.gameMode.PlaySound(clip);
    }
}
