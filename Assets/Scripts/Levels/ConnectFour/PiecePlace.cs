using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePlace : MonoBehaviour
{
    private float speed = .1f;
    private float distance = .1f;

    //if collided set piece to static and move to place
    void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        StartCoroutine(MoveToPlace(col.transform));
    }

    //move piece to place and run function in rowmanager
    IEnumerator MoveToPlace(Transform piece)
    {
        Debug.Log("here");
        while(this.transform.position.y - piece.position.y < distance)
        {
            piece.transform.position = Vector3.Lerp(piece.transform.position, this.transform.position, speed);
        }
        piece.transform.position = this.transform.position;
        GetComponentInParent<RowManager>().PiecePlaced();
        Debug.Log("here?");
        yield return null;
    }
}
