using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingPieceButton : MonoBehaviour
{
    public GameObject playerOnePiece;

    public GameObject piecesParent;

    //for disabling if its filled
    public GameObject topRowPosition;

    public void CheckTopPosition(bool last)
    {
        if(topRowPosition.GetComponent<PiecePlace>().redPlaced || topRowPosition.GetComponent<PiecePlace>().bluePlaced)
        {
            if(last)
            {
                ConnectManager.connectManager.StaleMate();
            }
            Destroy(gameObject);
        }
    }
    public void PlacePiece()
    {
        //place piece
        Instantiate(playerOnePiece, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity, piecesParent.transform);
        //disable buttons
        ConnectManager.connectManager.PieceStarted();
    }
}
