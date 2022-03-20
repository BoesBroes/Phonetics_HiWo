using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowManager : MonoBehaviour
{
    public GameObject[] rowPosition;

    public int currentRow = 0;

    public int rowNumber;
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
        //but dont disable last collider as its needed to check rows
        //rowPosition[currentRow].GetComponent<Collider2D>().enabled = false;
        ConnectManager.connectManager.CheckRow(rowPosition[currentRow], currentRow, rowNumber);
        currentRow++;
        rowPosition[currentRow].GetComponent<Collider2D>().enabled = true;

        //switch turns
    }
}
