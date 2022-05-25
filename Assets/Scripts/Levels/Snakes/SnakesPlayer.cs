using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakesPlayer : MonoBehaviour
{
    public int position;

    public GameObject[] board;

    public float speed;
    public float distance;
    private float timeElapsed;

    public AudioClip ladder;
    public AudioClip snakes;
    public AudioClip walk;
    public void MoveToNewPosition(GameObject destination, int rolled, bool closeToTheEnd)
    {
        if(closeToTheEnd)
        {
            position--;
        }
        else
        {
            position++;
        }
        StartCoroutine(LerpPosition(destination, closeToTheEnd));
    }

    IEnumerator LerpPosition(GameObject destination, bool closeToTheEnd)
    {
        timeElapsed = 0;
        SnakesManager.snakesManager.gameMode.PlaySound(walk);


        while (Mathf.Abs(this.transform.position.x - destination.transform.position.x) > distance && !closeToTheEnd || Mathf.Abs(this.transform.position.y - destination.transform.position.y) > distance && !closeToTheEnd)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, board[position].transform.position, timeElapsed / speed);
            timeElapsed = Time.fixedDeltaTime;
            if (board[position] != destination)
            {
                if (Mathf.Abs(this.transform.position.x - board[position].transform.position.x) < distance + 10f && Mathf.Abs(this.transform.position.y - board[position].transform.position.y) < distance + 10f)
                {
                    position++;
                    timeElapsed = 0;
                    SnakesManager.snakesManager.gameMode.PlaySound(walk);
                }
            }
            yield return null;
        }


        while (Mathf.Abs(this.transform.position.x - destination.transform.position.x) > distance && closeToTheEnd|| Mathf.Abs(this.transform.position.y - destination.transform.position.y) > distance && closeToTheEnd)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, board[position].transform.position, timeElapsed / speed);
            timeElapsed = Time.fixedDeltaTime;

            if (board[position] != destination)
            {
                if (Mathf.Abs(this.transform.position.x - board[position].transform.position.x) < distance + 10f && Mathf.Abs(this.transform.position.y - board[position].transform.position.y) < distance + 10f)
                {
                    position--;
                    timeElapsed = 0;
                    SnakesManager.snakesManager.gameMode.PlaySound(walk);
                }
            }

            yield return null;
        }


        this.transform.position = destination.transform.position;

        //if ladder or snek
        if(destination.GetComponent<SnakesPlace>().isLadder || destination.GetComponent<SnakesPlace>().isSnake)
        {
            if (destination.GetComponent<SnakesPlace>().isLadder)
            {
                SnakesManager.snakesManager.gameMode.PlaySound(ladder);
            }
            else
            {
                SnakesManager.snakesManager.gameMode.PlaySound(snakes);
            }

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
        position++;

        timeElapsed = 0;
        SnakesManager.snakesManager.gameMode.PlaySound(walk);

        while (Mathf.Abs(this.transform.position.x - destination.transform.position.x) > distance || Mathf.Abs(this.transform.position.y - destination.transform.position.y) > distance)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, board[position].transform.position, timeElapsed / speed);
            timeElapsed = Time.fixedDeltaTime;
            if (board[position] != destination)
            {
                if (Mathf.Abs(this.transform.position.x - board[position].transform.position.x) < distance + 10f && Mathf.Abs(this.transform.position.y - board[position].transform.position.y) < distance + 10f)
                {
                    position++;
                    timeElapsed = 0;
                    SnakesManager.snakesManager.gameMode.PlaySound(walk);
                }
            }
            yield return null;
        }
        this.transform.position = destination.transform.position;

        MoveToNewPosition(actualDestination, rolled, true);
    }

}
