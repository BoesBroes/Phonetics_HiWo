using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTask : TaskMain
{
    public Slider loadSlider;

    //no it doesnt actually change the speed the game loads
    public float loadSpeed;

    private float newFloat;
    private float timeElapsed;

    public override void StartTask()
    {
        newFloat += .2f;

        StopCoroutine(MoveLoad());
        StartCoroutine(MoveLoad());

        if(newFloat == 1)
        {
            gameMode.TaskFinished();
        }
    }

    IEnumerator MoveLoad()
    {
        while (loadSlider.value < newFloat - 0.005f)
        {
            loadSlider.value = Mathf.Lerp(loadSlider.value, newFloat, timeElapsed / loadSpeed);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        loadSlider.value = newFloat;
    }

}
