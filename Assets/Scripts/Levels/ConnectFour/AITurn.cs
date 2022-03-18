using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurn : MonoBehaviour
{
    public GameObject playerTwoPiece;
    public GameObject fakePiece;

    private bool thinking;

    private Transform[] placeLocations;

    void Start()
    {
        placeLocations = new Transform[this.transform.childCount];

        for(int i = 0; i < placeLocations.Length; i++)
        {
            placeLocations[i] = this.transform.GetChild(i);
        }
    }

    //choose random place and instantiate piece
    public void AIturn()
    {
        //thinking = true;
        StartCoroutine(RandomThink());

        

        //change turn
    }

    IEnumerator ThinkingHard()
    {
        yield return RandomThink();
        yield return RandomThink();
        yield return RandomThink();
        yield return RandomThink();

        PlacePiece();
    }

    private IEnumerator RandomThink()
    {
        int ran = Random.Range(0, 5);

        GameObject temp = Instantiate(fakePiece, new Vector3(placeLocations[ran].transform.position.x, placeLocations[ran].transform.position.y, 0), Quaternion.identity, placeLocations[ran].transform);
        yield return new WaitForSeconds(.25f);
        Destroy(temp);
    }

    private void PlacePiece()
    {
        int ran = Random.Range(0, 5);

        Instantiate(playerTwoPiece, new Vector3(placeLocations[ran].transform.position.x, placeLocations[ran].transform.position.y, 0), Quaternion.identity, placeLocations[ran].transform);
    }
}
