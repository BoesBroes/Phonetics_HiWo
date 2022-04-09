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
        SnailTask.snailTask.gameMode.PlaySound(diceRoll);

        Debug.Log("thrown!");

        playerTurn = player;

        diceRoller[0].RollDice();
        diceRoller[1].RollDice();
    }

    public void ThrowResult(int dice, int result)
    {
        numberOfResults++;

        if (numberOfResults <= 1)
        {
            resultOne = result;
            SnailTask.snailTask.gameMode.PlaySound(diceResult);
        }
        else
        {
            resultTwo = result;

            if (!playerTurn)
            {
                Debug.Log("AI");
                SnailTask.snailTask.AIDecision(resultOne, resultTwo);
            }
            else
            {
                Debug.Log("player");
                SnailTask.snailTask.ActivateButtons(resultOne, resultTwo);
            }
            numberOfResults = 0;
        }

    }
}
