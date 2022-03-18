using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowManager : MonoBehaviour
{
    public GameObject[] rowPosition;

    private int currentRow = 0;
    // Start is called before the first frame update
    void Start()
    {
        //initialize all possible positions
        rowPosition = new GameObject[this.transform.childCount];
        for(int i = 0; i < rowPosition.Length; i++)
        {
            rowPosition[i] = this.transform.GetChild(i).gameObject;
            rowPosition[i].GetComponent<Collider2D>().enabled = false;
        }
        rowPosition[0].GetComponent<Collider2D>().enabled = true;
    }

    public void PiecePlaced()
    {
        //move the collider one place up
        rowPosition[currentRow].GetComponent<Collider2D>().enabled = false;
        currentRow++;
        rowPosition[currentRow].GetComponent<Collider2D>().enabled = true;

        //switch turns
        ConnectManager.connectManager.SwitchTurn();
    }
}
