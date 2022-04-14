using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakesDice : MonoBehaviour
{
    public Text diceText;

    public int dice;

    public SnakesManager snakeManager;

    public void RollDice()
    {
        if(snakeManager.playerTurn)
        {
            snakeManager.playerTurn = false;
            StartCoroutine(Rolling());
        }
    }

    public void AIDiceRoll()
    {
        StartCoroutine(Rolling());
    }

    IEnumerator Rolling()
    {
        Result(false);
        yield return new WaitForSeconds(Random.Range(.1f, .5f));

        Result(false);
        yield return new WaitForSeconds(.2f);

        Result(false);
        yield return new WaitForSeconds(.1f);

        Result(false);
        yield return new WaitForSeconds(.3f);

        Result(false);
        yield return new WaitForSeconds(.2f);

        Result(false);
        yield return new WaitForSeconds(.2f);

        Result(false);
        yield return new WaitForSeconds(.4f);

        Result(true);
        yield return new WaitForSeconds(.4f);
    }

    private void Result(bool final)
    {
        int temp = Random.Range(0, 6);

        //for ease of use, in same order as the board
        switch (temp)
        {
            case 0:
                diceText.text = "1";
                break;
            case 1:
                diceText.text = "2";
                break;
            case 2:
                diceText.text = "3";
                break;
            case 3:
                diceText.text = "4";
                break;
            case 4:
                diceText.text = "5";
                break;
            case 5:
                diceText.text = "6";
                break;
        }

        if (final)
        {
            snakeManager.MoveCurrentPlayer(temp + 1);
        }
    }
}
