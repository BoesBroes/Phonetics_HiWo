using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectManager : TaskMain
{
    public GameObject playerTurn;
    public GameObject computerTurn;

    public static ConnectManager connectManager;

    private bool won = false;

    public GameObject board;

    private int redRowCount = 0;
    private int blueRowCount = 0;

    //count how many rows exist on the left
    private int diagonalCount;

    //ints to avoid out of bounds
    private int bounds;
    private int boundsCorrection;

    //used for storing current position, place in row and row
    private GameObject currentLocation;
    private int currentRowPlace;
    private int currentRowNumber;

    private bool turnPlayer = true;

    public AudioClip pieceCollisionSound;

    public WordImage wordImage;

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
        //store each location relative to current location in row, row in width and height respectively
        currentLocation = currentPosition;
        currentRowPlace = currentRow;
        currentRowNumber = rowNumber;

        StartCoroutine(CollisionSoundWait());
    }

    IEnumerator CollisionSoundWait()
    {
        gameMode.PlaySound(pieceCollisionSound);
        yield return new WaitForSeconds(pieceCollisionSound.length);

        //show image on screen
        wordImage.gameObject.SetActive(true);
        wordImage.FindImage(currentLocation.GetComponent<WordObject>().word[0]);

        RecognitionManager.recognitionManager.RecognizeWord(currentLocation, turnPlayer);
    }

    public override void Proceed()
    {
        base.Proceed();

        //remove image on screen
        wordImage.gameObject.SetActive(false);

        //check each location
        CheckVertical(currentLocation);
        CheckHorizontal(currentLocation, currentRowPlace);
        CheckDiagonal(currentLocation, currentRowPlace, currentRowNumber);
        if (!won)
        {
            SwitchTurn();
        }
    }

    private void CheckVertical(GameObject currentPosition)
    {
        GameObject currentRow = currentPosition.transform.parent.gameObject;
        GameObject[] places = currentRow.GetComponent<RowManager>().rowPosition;

        CheckPlaces(places, false);
    }


    private void CheckHorizontal(GameObject currentPosition, int currentRow)
    {
        GameObject[] places = new GameObject[board.transform.childCount];

        for (int i = 0; i < places.Length; i++)
        {
            places[i] = board.transform.GetChild(i).gameObject.GetComponent<RowManager>().rowPosition[currentRow];
        }

        CheckPlaces(places, false);
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
        CheckPlaces(places, true);

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
        CheckPlaces(places, true);

    }

    private void CheckPlaces(GameObject[] places, bool checkingDiagonal)
    {
        if(diagonalCount >= 0 && checkingDiagonal)
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
            //check which one is placed at x position
            //if red is placed + check if last was blue
            if (places[i].GetComponent<PiecePlace>().redPlaced && blueRowCount == 0)
            {
                redRowCount++;
            }
            else if (places[i].GetComponent<PiecePlace>().redPlaced && blueRowCount > 0)
            {
                blueRowCount = 0;
                redRowCount++;
            }

            //if blue is placed + check if last was red
            else if (places[i].GetComponent<PiecePlace>().bluePlaced && redRowCount == 0)
            {
                blueRowCount++;
            }
            else if (places[i].GetComponent<PiecePlace>().bluePlaced && redRowCount > 0)
            {
                redRowCount = 0;
                blueRowCount++;
            }

            //if none are placed
            else
            {
                redRowCount = 0;
                blueRowCount = 0;
            }

            if (redRowCount == 4)
            {
                won = false;
                Debug.Log("insert winning condition");
                gameMode.lose = true;
                gameMode.EndLevel();
                this.gameObject.SetActive(false);
            }
            if (blueRowCount == 4)
            {
                won = true;
                Debug.Log("insert winning condition");
                gameMode.win = true;
                gameMode.EndLevel();
                this.gameObject.SetActive(false);
            }
        }

        if(diagonalCount >= 0 && checkingDiagonal)
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

            //check for each row if the last position is filled
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
            turnPlayer = false;

            computerTurn.GetComponent<AITurn>().AIStartturn();
        }
        else if (computerTurn.transform.childCount == 0)
        {
            return;
        }
        else
        {
            turnPlayer = true;

            playerTurn.SetActive(true);

            //check for each row if the last position is filled
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
