using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public DiceRoller[] diceRoller;

    public string[] results;

    public GameObject buttonObject;

    private int numberOfResults;
    private int resultOne;
    private int resultTwo;

    private bool playerTurn;

    public AudioClip diceRoll;
    public AudioClip diceResult;

    //just to be sure it doesnt go wrong here
    private bool noNewResults;
    private bool firstNumberIn;
    private bool secondNumberIn;

    public void ChangePlayerTurn()
    {
        if(!buttonObject.activeSelf)
        {
            buttonObject.SetActive(true);
        }
        else
        {
            buttonObject.SetActive(false);
        }
    }
    public void ThrowDice(bool player)
    {
        noNewResults = false;

        firstNumberIn = false;
        secondNumberIn = false;

        SnailTask.snailTask.gameMode.PlaySound(diceRoll);

        playerTurn = player;

        diceRoller[0].RollDice(1);
        diceRoller[1].RollDice(2);
    }

    public void ThrowResult(int dice, int result, int resultNumber)
    {
        numberOfResults++;

        if (resultNumber == 1 && !firstNumberIn)
        {
            resultOne = result;
            SnailTask.snailTask.gameMode.PlaySound(diceResult);

            firstNumberIn = true;
        }
        else if(resultNumber == 2 && !secondNumberIn)
        {
            resultTwo = result;
            SnailTask.snailTask.gameMode.PlaySound(diceResult);

            secondNumberIn = true;
        }

        if(numberOfResults >= 1 && !noNewResults && firstNumberIn && secondNumberIn)
        {
            noNewResults = true;
            StartCoroutine(WaitForSound());

            numberOfResults = 0;
        }

    }

    IEnumerator WaitForSound()
    {
        yield return new WaitForSeconds(diceResult.length);

        Debug.Log(resultOne);
        Debug.Log(resultTwo);


        if (!playerTurn)
        {
            SnailTask.snailTask.AIDecision(resultOne, resultTwo);
        }
        else
        {
            SnailTask.snailTask.ActivateButtons(resultOne, resultTwo);
        }
        numberOfResults = 0;
    }
}
