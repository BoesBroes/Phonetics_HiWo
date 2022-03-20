using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingPieceButton : MonoBehaviour
{
    public GameObject playerOnePiece;

    public GameObject piecesParent;

    public void PlacePiece()
    {
        //place piece
        Instantiate(playerOnePiece, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity, piecesParent.transform);
        //disable buttons
        ConnectManager.connectManager.PieceStarted();
    }
}
