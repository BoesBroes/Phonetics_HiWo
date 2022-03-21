using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePosition : MonoBehaviour
{
    //for disabling positions if row filled
    public GameObject topRowPosition;

    //destroy this to disable location for spawning pieces
    public void CheckTopPosition(bool last)
    {
        if (topRowPosition.GetComponent<PiecePlace>().redPlaced || topRowPosition.GetComponent<PiecePlace>().bluePlaced)
        {
            if (!last)
            {
                transform.parent.GetComponent<AITurn>().ReInitialize(this.gameObject);
            }
            else
            {
                ConnectManager.connectManager.StaleMate();
            }
            Destroy(gameObject);
        }
    }

}
