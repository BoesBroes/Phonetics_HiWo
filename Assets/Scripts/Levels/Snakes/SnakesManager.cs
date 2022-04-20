using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakesManager : TaskMain
{
    public static SnakesManager snakesManager;

    public GameObject board;
    public SnakesPlace[] places;
    public string[] words;

    public SnakesDice snakesDice;

    public bool playerTurn;
    public SnakesPlayer[] players;
    private int turn = 0;

    public WordImage wordImage;


    void Awake()
    {
        if (snakesManager == null)
        {
            snakesManager = this;
        }
        else
        {
            Destroy(snakesManager);
            snakesManager = this;
        }
    }

    //stuff like this generally should be hidden until its loaded (but timelimit)
    public override void StartTask()
    {
        base.StartTask();

        playerTurn = true;

        places = new SnakesPlace[board.transform.childCount];
        for(int i = 0; i < places.Length; i++)
        {
            places[i] = board.transform.GetChild(i).GetComponent<SnakesPlace>();
        }

        List<int> listNumbers = new List<int>();
        int number;

        //find 49 random words
        for (int i = 0; i < places.Length; i++)
        {
            do
            {
                number = Random.Range(0, words.Length);
            }
            while
            (listNumbers.Contains(number));
            listNumbers.Add(number);
        }

        //set random words in order of places
        for (int i = 0; i < places.Length; i++)
        {
            places[i].word = words[listNumbers[i]];
            places[i].SetImage();
        }
    }


    public void MoveCurrentPlayer(int rolled)
    {
        if (rolled + players[turn].position > places.Length - 1)
        {
            rolled = ((places.Length - 1) - (players[turn].position + rolled)) + ((places.Length - 1) - players[turn].position);
            players[turn].MoveToFakeEnd(places[places.Length - 1].gameObject, places[rolled + players[turn].position].gameObject, rolled);
            Debug.Log(rolled);
        }
        else
        {
            players[turn].MoveToNewPosition(places[rolled + players[turn].position].gameObject, rolled);
        }
    }

    public void PlayerMoved()
    {
        if (players[turn].position == places.Length - 1)
        {
            if (turn == 0)
            {
                gameMode.win = true;
            }
            else
            {
                gameMode.lose = true;
            }

            gameMode.EndLevel();
            return;
        }

        wordImage.gameObject.SetActive(true);
        wordImage.FindImage(places[players[turn].position].word);

        if (turn == 0)
        {
            RecognitionManager.recognitionManager.RecognizeWord(places[players[turn].position].gameObject, true);
        }
        else
        {
            RecognitionManager.recognitionManager.RecognizeWord(places[players[turn].position].gameObject, false);
        }
    }

    public override void Proceed()
    {
        base.Proceed();

        wordImage.gameObject.SetActive(false);

        turn++;
        if (turn > 3)
        {
            turn = 0;
            playerTurn = true;
        }
        else
        {
            TurnAI();
        }
        
    }

    private void TurnAI()
    {
        snakesDice.AIDiceRoll();
    }

}
