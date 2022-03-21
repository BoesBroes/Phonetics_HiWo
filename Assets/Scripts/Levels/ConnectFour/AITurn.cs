using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurn : MonoBehaviour
{
    public GameObject playerTwoPiece;
    public GameObject fakePiece;
    public GameObject piecesParent;

    private bool thinking;

    private Transform[] placeLocations;

    private int max;

    void Start()
    {
        max = this.transform.childCount - 1;

        placeLocations = new Transform[this.transform.childCount];

        for(int i = 0; i < placeLocations.Length; i++)
        {
            placeLocations[i] = this.transform.GetChild(i);
        }
    }

    //set the length of the array of rows again if one is removed due to being full
    public void ReInitialize(GameObject removedObject)
    {
        //move last un-empty row in list to full row
        for (int i = 0; i < placeLocations.Length; i++)
        {
            if (placeLocations[i].gameObject == removedObject)
            {
                placeLocations[i] = placeLocations[placeLocations.Length - 1];
            }
        }
        Transform[] temp = new Transform[placeLocations.Length - 1];
        temp = placeLocations;
        placeLocations = new Transform[placeLocations.Length - 1];
        placeLocations = temp;

        max = this.transform.childCount - 1;
    }


    //choose random place and instantiate piece
    public void AIStartturn()
    {
        //thinking = true;
        StartCoroutine(ThinkingHard());

        

        //change turn
    }

    IEnumerator ThinkingHard()
    {
        yield return RandomThink();
        yield return new WaitForSeconds(.5f);

        yield return RandomThink();
        yield return new WaitForSeconds(.25f);

        yield return RandomThink();
        yield return new WaitForSeconds(.5f);

        yield return RandomThink();
        yield return new WaitForSeconds(.25f);


        PlacePiece();
    }

    private IEnumerator RandomThink()
    {
        int ran = Random.Range(0, max);
        
        GameObject temp = Instantiate(fakePiece, new Vector3(placeLocations[ran].transform.position.x, placeLocations[ran].transform.position.y, 0), Quaternion.identity, placeLocations[ran].transform);
        yield return new WaitForSeconds(.5f);
        Destroy(temp);
        yield return null;
    }

    private void PlacePiece()
    {
        int ran = Random.Range(0, max);

        Instantiate(playerTwoPiece, new Vector3(placeLocations[ran].transform.position.x, placeLocations[ran].transform.position.y, 0), Quaternion.identity, piecesParent.transform);

        ConnectManager.connectManager.PieceStarted();
    }
}
