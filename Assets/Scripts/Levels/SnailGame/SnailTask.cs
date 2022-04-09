using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailTask : TaskMain
{
    public static SnailTask snailTask;

    public GameObject board;
    private TrackManager[] tracks;

    private int turnCount;

    //public int diceOneResult;
    //public int diceTwoResult;

    //for throwing dice
    public DiceManager diceManager;
    //for showing words
    public WordRepeater wordRepeater;
    //the win screen
    public ColorWin colorWin;

    public AudioClip walkScrape;
    void Awake()
    {
        if (snailTask == null)
        {
            snailTask = this;
        }
        else
        {
            Destroy(snailTask);
            snailTask = this;
        }
    }

    public override void StartTask()
    {
        tracks = new TrackManager[board.transform.childCount];
        for(int i = 0; i < tracks.Length; i++)
        {
            tracks[i] = board.transform.GetChild(i).GetComponent<TrackManager>();
        }
        //decide who turns starts
        
    }

    public void AIDecision(int resultone, int resultTwo)
    {
        int temp = Random.Range(0, 1);
        
        if(resultone == resultTwo)
        {
            tracks[resultone].MovePiece(true);
        }

        else if(temp == 0)
        {
            tracks[resultone].MovePiece(false);
        }

        else
        {
            tracks[resultTwo].MovePiece(false);
        }
    }

    public void ActivateButtons(int resultone, int resultTwo)
    {
        tracks[resultone].ActivateButton();
        tracks[resultTwo].ActivateButton();
    }

    public void DeactivateAllButtons()
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            tracks[i].DeactivateButton();
        }
    }

    public void NextTurn()
    {
        turnCount++;
        if(turnCount > 5)
        {
            turnCount = 0;
        }
        if(turnCount == 0)
        {
            diceManager.ChangePlayerTurn();
        }
        else if (turnCount == 1)
        {
            wordRepeater.StartWord();
        }
        else
        {
            diceManager.ThrowDice(false);
        }
    }

    public override void Proceed()
    {
        base.Proceed();
        wordRepeater.StopWord();
        diceManager.ThrowDice(false);
    }

    public void CheckWin(int positionNumber, int player)
    {
        if(positionNumber == 12)
        {
            //a color always wins
            colorWin.ChangeText(player);

            gameMode.win = true;
            gameMode.EndLevel();
        }
        else
        {
            NextTurn();
        }
    }

    public void PlaySoundWalk(AudioClip clip)
    {
        gameMode.PlaySound(clip);
    }
}
