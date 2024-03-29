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
    public GameObject wordRepeat;

    public AudioClip yourTurn;

    public Hint hint;
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

        hint.gameObject.SetActive(true);

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

        if (gameMode.audioSource.isPlaying)
        {
            StartCoroutine(hint.WaitForStartHint());
        }
        else
        {
            hint.GiveHint();
        }
    }


    public void MoveCurrentPlayer(int rolled)
    {
        //Debug.Log(rolled + players[turn].position);
        //if the rolled number + currentposition is higher than the number of positions, the player will move to the finish and back
        if (rolled + players[turn].position > places.Length - 1)
        {
            //places.length - 1 because the array starts at 0 (and is considered 1 length)
            rolled = ((places.Length - 1) - (players[turn].position + rolled)) + ((places.Length - 1) - players[turn].position);
            players[turn].MoveToFakeEnd(places[places.Length - 1].gameObject, places[rolled + players[turn].position].gameObject, rolled);
        }
        else
        {
            players[turn].MoveToNewPosition(places[rolled + players[turn].position].gameObject, rolled, false);
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

        wordRepeat.SetActive(true);
        wordImage.FindImage(places[players[turn].position].word);

        if (turn == 0)
        {
            TaskGameMode.gameMode.noHints = true;
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

        wordRepeat.gameObject.SetActive(false);

        turn++;
        if (turn > 3)
        {
            turn = 0;
            playerTurn = true;
            gameMode.PlaySound(yourTurn);
            gameMode.noHints = false;
        }
        else
        {
            TurnAI();
            gameMode.noHints = true;
        }

    }

    private void TurnAI()
    {
        snakesDice.AIDiceRoll();
    }

}
