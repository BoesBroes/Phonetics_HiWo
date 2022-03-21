using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectManager : TaskMain
{
    public GameObject playerTurn;
    public GameObject computerTurn;

    public static ConnectManager connectManager;

    public bool won = false;

    public GameObject board;

    private int redRowCount = 0;
    private int blueRowCount = 0;

    //count how many rows exist on the left
    private int diagonalCount;

    //ints to avoid out of bounds
    private int bounds;
    private int boundsCorrection;
    void Awake()
    {
        if(connectManager == null)
        {
            connectManager = this;
        }
        else
        {
            Destroy(connectManager);
            connectManager = this;
        }
    }

    // Start is called before the first frame update
    public override void StartTask()
    {
        playerTurn.SetActive(true);
        computerTurn.SetActive(false);
    }

    public void CheckRow(GameObject currentPosition, int currentRow, int rowNumber)
    {
        CheckVertical(currentPosition);
        CheckHorizontal(currentPosition, currentRow);
        CheckDiagonal(currentPosition, currentRow, rowNumber);
        if(!won)
        {
            SwitchTurn();
        }
    }

    private void CheckVertical(GameObject currentPosition)
    {
        GameObject currentRow = currentPosition.transform.parent.gameObject;
        GameObject[] places = currentRow.GetComponent<RowManager>().rowPosition;

        CheckPlaces(places);
    }


    private void CheckHorizontal(GameObject currentPosition, int currentRow)
    {
        GameObject[] places = new GameObject[board.transform.childCount];

        for (int i = 0; i < places.Length; i++)
        {
            places[i] = board.transform.GetChild(i).gameObject.GetComponent<RowManager>().rowPosition[currentRow];
        }

        CheckPlaces(places);
    }

    private void CheckDiagonal(GameObject currentPosition, int currentRowCount, int rowNumber)
    {
        GameObject[] places = new GameObject[6];

        //set boundaries for going to the right
        //makes sure the rows x and y are not below 0 or above 5 respectively 
        bounds = rowNumber - currentRowCount;
        boundsCorrection = 0;

        if (bounds < 0)
        {
            boundsCorrection = bounds;
            bounds = 0;
        }

        //check diagonal going righttop
        for (int i = 0; i < places.Length; i++)
        {
            diagonalCount++;
            places[i] = board.transform.GetChild(i + (bounds)).gameObject.GetComponentInChildren<RowManager>().rowPosition[i - boundsCorrection];
            if ((i + 1) + (bounds) > 6 || i - boundsCorrection > 4)
            {
                break;
            }
            else
            {
            }
        }
        CheckPlaces(places);

        places = new GameObject[6];

        //set boundaries for going to the left
        //makes sure the rows x and y are not above 6 and above 5 respectively
        bounds = rowNumber + currentRowCount;
        boundsCorrection = 0;

        if (bounds > 6)
        {
            boundsCorrection = bounds - 6;
            bounds = 6;
        }

        //check diagonal going lefttop
        for (int i = 0; i < places.Length; i++)
        {
            diagonalCount++;
            places[i] = board.transform.GetChild(-i + bounds).gameObject.GetComponentInChildren<RowManager>().rowPosition[i + boundsCorrection];
            if((-i - 1) + (bounds - boundsCorrection) < 0 || i + boundsCorrection > 4)
            {
                break;
            }
            else
            {
            }

        }
        CheckPlaces(places);

    }

    private void CheckPlaces(GameObject[] places)
    {
        if(diagonalCount >= 0)
        {
            //shorten places array
            GameObject[] temp = places;
            places = new GameObject[diagonalCount];
            for(int i = 0; i < places.Length; i++)
            {
                places[i] = temp[i];
            }
        }
        for (int i = 0; i < places.Length; i++)
        {
            if (places[i].GetComponent<PiecePlace>().redPlaced && blueRowCount == 0)
            {
                redRowCount++;
            }
            if (places[i].GetComponent<PiecePlace>().redPlaced && blueRowCount > 0)
            {
                blueRowCount = 0;
                redRowCount++;
            }

            if (places[i].GetComponent<PiecePlace>().bluePlaced && redRowCount == 0)
            {
                blueRowCount++;
            }
            if (places[i].GetComponent<PiecePlace>().bluePlaced && redRowCount > 0)
            {
                redRowCount = 0;
                blueRowCount++;
            }

            if (redRowCount == 4)
            {
                won = false;
                Debug.Log("insert winning condition");
                gameMode.lose = true;
                gameMode.EndLevel();
            }
            if (blueRowCount == 4)
            {
                won = true;
                Debug.Log("insert winning condition");
                gameMode.win = true;
                gameMode.EndLevel();
            }
        }

        if(diagonalCount >= 0)
        {
            diagonalCount = -1;
        }

        redRowCount = 0;
        blueRowCount = 0;
    }

    public void StaleMate()
    {
        gameMode.stale = true;
        gameMode.EndLevel();
        this.gameObject.SetActive(false);
    }

    public void PieceStarted()
    {
        if (playerTurn.activeSelf)
        {
            playerTurn.SetActive(false);

            computerTurn.SetActive(true);

            if (computerTurn.transform.childCount > 1)
            {
                for (int i = 0; i < computerTurn.transform.childCount; i++)
                {
                    computerTurn.transform.GetChild(i).GetComponent<DisablePosition>().CheckTopPosition(false);
                }
            }
            else
            {
                computerTurn.transform.GetChild(0).GetComponent<DisablePosition>().CheckTopPosition(true);
            }
        }
        else
        {
            //playerTurn.SetActive(true);
            computerTurn.SetActive(false);
        }
    }

    public void SwitchTurn()
    {
        //switch turns between player and AI
        //TODO: add option for extra real player
        if (computerTurn.activeSelf && computerTurn.transform.childCount > 0)
        {
            computerTurn.GetComponent<AITurn>().AIStartturn();
        }
        else if (computerTurn.transform.childCount == 0)
        {
            return;
        }
        else
        {
            playerTurn.SetActive(true);
            if (playerTurn.transform.childCount > 1)
            {
                for (int i = 0; i < playerTurn.transform.childCount; i++)
                {
                    playerTurn.transform.GetChild(i).GetComponent<PlacingPieceButton>().CheckTopPosition(false);
                }
            }
            else
            {
                playerTurn.transform.GetChild(0).GetComponent<PlacingPieceButton>().CheckTopPosition(true);
            }
        }
    }
}
