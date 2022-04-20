using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakesPlayer : MonoBehaviour
{
    public int position;


    public float speed;
    public float distance;
    private float timeElapsed;

    public void MoveToNewPosition(GameObject destination, int rolled)
    {
        position += rolled;
        StartCoroutine(LerpPosition(destination));
    }

    IEnumerator LerpPosition(GameObject destination)
    {
        timeElapsed = 0;
        while (Mathf.Abs(this.transform.position.x - destination.transform.position.x) > distance || Mathf.Abs(this.transform.position.y - destination.transform.position.y) > distance)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, destination.transform.position, timeElapsed / speed);
            timeElapsed = Time.fixedDeltaTime;
            yield return null;
        }
        this.transform.position = destination.transform.position;

        //if ladder or snek
        if(destination.GetComponent<SnakesPlace>().isLadder || destination.GetComponent<SnakesPlace>().isSnake)
        {
            position += destination.GetComponent<SnakesPlace>().steps;

            destination = SnakesManager.snakesManager.places[position].gameObject;

            timeElapsed = 0;
            while (Mathf.Abs(this.transform.position.x - destination.transform.position.x) > distance || Mathf.Abs(this.transform.position.y - destination.transform.position.y) > distance)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, destination.transform.position, timeElapsed / speed);
                timeElapsed = Time.fixedDeltaTime;
                yield return null;
            }
            this.transform.position = destination.transform.position;
        }

        SnakesManager.snakesManager.PlayerMoved();
    }


    public void MoveToFakeEnd(GameObject destination, GameObject actualDestination, int rolled)
    {
        StartCoroutine(LerpFakeEndPosition(destination, actualDestination, rolled));
    }

    //moves to the end first before moving back
    IEnumerator LerpFakeEndPosition(GameObject destination, GameObject actualDestination, int rolled)
    {
        timeElapsed = 0;
        while (Mathf.Abs(this.transform.position.x - destination.transform.position.x) > distance || Mathf.Abs(this.transform.position.y - destination.transform.position.y) > distance)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, destination.transform.position, timeElapsed / speed);
            timeElapsed = Time.fixedDeltaTime;
            yield return null;
        }
        this.transform.position = destination.transform.position;

        MoveToNewPosition(actualDestination, rolled);
    }

}
