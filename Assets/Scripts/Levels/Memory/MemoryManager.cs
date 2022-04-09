using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryManager : TaskMain
{
    public RepeatMemory repeatMemory;

    public string[] allWords;
 
    public GameObject board;
    private ShowImage[] cards;


    public static MemoryManager memoryManager;

    public int clicks;

    private ShowImage[] currentImageObjects;

    private int cardsLeft;
    private int score;
    private int aiScore;
    public Text scoreText;

    public bool playerTurn;

    private int cardCount;

    private int[] cardAIFlipCount;

    void Awake()
    {
        if (memoryManager == null)
        {
            memoryManager = this;
        }
        else
        {
            Destroy(memoryManager);
            memoryManager = this;
        }
    }

    // Start is called before the first frame update
    public override void StartTask()
    {
        playerTurn = true;

        cardAIFlipCount = new int[2];

        currentImageObjects = new ShowImage[2];

        //define all cards
        cards = new ShowImage[board.transform.childCount];
        cardsLeft = board.transform.childCount;

        for (int i = 0; i < board.transform.childCount; i++)
        {
            cards[i] = board.transform.GetChild(i).GetComponent<ShowImage>();
        }

        List<int> listNumbers = new List<int>();
        int number;

        //find 12 random words
        for (int i = 0; i < cards.Length / 2; i++)
        {
            do
            {
                number = Random.Range(0, allWords.Length);
            }
            while 
            (listNumbers.Contains(number));
            listNumbers.Add(number);
        }

        //first set original words in order of cards
        for(int i = 0; i < cards.Length / 2; i++)
        {
            cards[i].word = allWords[listNumbers[i]];
        }

        List<int> listRan = new List<int>();
        int ranNum;

        //set ran numbers to 12
        for (int i = 0; i < cards.Length / 2; i++)
        {
            do
            {
                ranNum = Random.Range(0, 12);
            }
            while
            (listRan.Contains(ranNum));
            listRan.Add(ranNum);
        }

        //set duplicate word to card
        for (int i = 0; i < cards.Length / 2; i++)
        {
            cards[listRan[i] + 12].word = allWords[listNumbers[i]];
        }
    }

    public void ImageClicked(ShowImage imageObject)
    {
        currentImageObjects[clicks] = imageObject;

        clicks++;
        if(clicks == 2)
        {
            CheckWord();
        }
    }

    private void CheckWord()
    {
        if (currentImageObjects[0].word == currentImageObjects[1].word)
        {
            //increase score and add extra card to make it go to the next turn in Proceed
            score++;
            cardCount++;
            Proceed();
        }
        else
        {
            if (playerTurn)
            {
                repeatMemory.StartWordPlayer(currentImageObjects[0].word, currentImageObjects[0].wordImage);
            }
            else
            {
                repeatMemory.StartWordAI(currentImageObjects[0].word, currentImageObjects[0].wordImage);
            }


        }
    }

    public override void Proceed()
    {
        base.Proceed();

        cardCount++;

        //play a sound or two here before starting the coroutine

        repeatMemory.DisableImage();

        if (cardCount == 2)
        {
            cardCount = 0;

            cardsLeft -= 2;

            if (cardsLeft > 0)
            {

                if (playerTurn)
                {
                    StartCoroutine(ShowImagesTime(2.5f));
                }
                else
                {
                    StartCoroutine(ShowImagesTime(5f));
                }
            }
            else
            {
                if (score > aiScore)
                {
                    gameMode.win = true;
                }
                else if (score == aiScore)
                {
                    gameMode.stale = true;
                }
                else
                {
                    gameMode.lose = true;
                }
                gameMode.EndLevel();
            }
        }
        else
        {
            if (playerTurn)
            {
                repeatMemory.StartWordPlayer(currentImageObjects[1].word, currentImageObjects[1].wordImage);
            }
            else
            {
                repeatMemory.StartWordAI(currentImageObjects[1].word, currentImageObjects[1].wordImage);
            }
        }
    }

    IEnumerator ShowImagesTime(float time)
    {
        //make sure the user can see the images
        yield return new WaitForSeconds(time);

        if (currentImageObjects[0].word == currentImageObjects[1].word)
        {
            currentImageObjects[0].transform.parent = this.transform;
            currentImageObjects[1].transform.parent = this.transform;
            Destroy(currentImageObjects[0].gameObject);
            Destroy(currentImageObjects[1].gameObject);
        }
        else
        {
            currentImageObjects[0].DisableImage();
            currentImageObjects[1].DisableImage();
        }

        clicks = 0;

        if(playerTurn)
        {
            playerTurn = false;
            AITurn();
        }
        else
        {
            playerTurn = true;
        }
    }

    private void AITurn()
    {
        List<int> listNumbers = new List<int>();
        int number;

        for (int i = 0; i < 2; i++)
        {
            do
            {
                number = Random.Range(0, board.transform.childCount);
            }
            while
            (listNumbers.Contains(number));
            listNumbers.Add(number);
        }

        for (int i = 0; i < 2; i++)
        {
            cardAIFlipCount[i] = listNumbers[i];

            //board.transform.GetChild(listNumbers[i]).GetComponent<ShowImage>().AICheckImage();
        }

        StartCoroutine(AIFlipTime());
    }

    IEnumerator AIFlipTime()
    {
        board.transform.GetChild(cardAIFlipCount[0]).GetComponent<ShowImage>().AICheckImage();

        yield return new WaitForSeconds(1);

        board.transform.GetChild(cardAIFlipCount[1]).GetComponent<ShowImage>().AICheckImage();
    }

    public void ImageClickedAI(ShowImage imageObject)
    {
        currentImageObjects[clicks] = imageObject;

        clicks++;
        if (clicks == 2)
        {
            CheckWord();
        }
    }
}
