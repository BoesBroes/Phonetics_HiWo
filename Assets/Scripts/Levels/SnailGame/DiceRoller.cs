using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
    public Image thisImage;

    public int dice;

    public DiceManager diceManager;
    public void RollDice()
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
                thisImage.color = Color.blue;
                break;
            case 1:
                thisImage.color = Color.yellow;
                break;
            case 2:
                thisImage.color = Color.white;
                break;
            case 3:
                thisImage.color = Color.red;
                break;
            case 4:
                thisImage.color = Color.green;
                break;
            case 5:
                thisImage.color = Color.black; //find orange here
                break;
        }

        if(final)
        {
            diceManager.ThrowResult(dice, temp);
        }    
    }
}
