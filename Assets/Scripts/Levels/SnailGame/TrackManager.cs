using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public float speed = 0.5f;
    private float distance = 0.5f;

    public int colorInt;
    public GameObject buttonObject;

    public GameObject piece;

    public GameObject positionsParent;
    private GameObject[] positions;
    private int currentPosition = 0;

    private int steps = 1;
    // Start is called before the first frame update
    void Start()
    {
        positions = new GameObject[positionsParent.transform.childCount];

        //define the track
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = positionsParent.transform.GetChild(i).gameObject;
        }
    }

    public void MovePiece(bool extra)
    {
        //if AI has 2 of the same colors
        if(extra)
        {
            steps = 2;
        }
        currentPosition += steps;
        StartCoroutine(MoveToPlace(positions[currentPosition].transform));

        SnailTask.snailTask.CheckWin(currentPosition, colorInt);
    }

    IEnumerator MoveToPlace(Transform destination)
    {
        while (destination.position.x - piece.transform.position.x < distance)
        {
            piece.transform.position = Vector3.Lerp(piece.transform.position, destination.position, speed);
            yield return null;
        }
        piece.transform.position = destination.position;
    }

    public void ActivateButton()
    {
        if(!buttonObject.activeSelf)
        {
            buttonObject.SetActive(true);
            steps = 1;
        }
        //if activated twice (thus two of the same dice)
        else
        {
            steps = 2;
        }
    }

    public void DeactivateButton()
    {
        buttonObject.SetActive(false);
    }
}
