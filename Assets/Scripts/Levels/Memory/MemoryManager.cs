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

    public AudioClip[] correct;
    public AudioClip yourTurn;

    public bool playerTurn;

    private int cardCount;

    private int[] cardAIFlipCount;

    //if right cards, turn remains
    private bool rightCards;

    public Hint hint;
    public ObtainedCards[] obtainedCards;

    public Text[] endText;
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
        hint.gameObject.SetActive(true);

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

        //this one used to assign it to random cards
        List<int> cardNumbers = new List<int>();
        int cardNum;

        for (int i = 0; i < 24; i++)
        {
            do
            {
                cardNum = Random.Range(0, 24);
            }
            while
            (cardNumbers.Contains(cardNum));
            cardNumbers.Add(cardNum);
        }

        //first set original words in order of cards
        for (int i = 0; i < cards.Length / 2; i++)
        {
            cards[cardNumbers[i]].word = allWords[listNumbers[i]];
        }

        //List<int> listRan = new List<int>();
        //int ranNum;

        ////set ran numbers to 12
        //for (int i = 0; i < cards.Length / 2; i++)
        //{
        //    do
        //    {
        //        ranNum = Random.Range(0, 12);
        //    }
        //    while
        //    (listRan.Contains(ranNum));
        //    listRan.Add(ranNum);
        //}

        //set duplicate word to card
        for (int i = 0; i < cards.Length / 2; i++)
        {
            cards[cardNumbers[i + 12]].word = allWords[listNumbers[i]];
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
            gameMode.StartMultipleSounds(correct);

            //increase score and add extra card to make it go to the next turn in Proceed
            cardCount++;
            rightCards = true;
            if(playerTurn)
            {
                score++;
                obtainedCards[0].CardObtained(currentImageObjects[0].wordImage.sprite);
            }
            else
            {
                aiScore++;
                obtainedCards[1].CardObtained(currentImageObjects[0].wordImage.sprite);
            }

            //2 cards leave the board (soon)
            cardsLeft -= 2;

            Proceed();
        }
        else
        {
            gameMode.noHints = true;

            rightCards = false;
            if (playerTurn)
            {
                repeatMemory.gameObject.SetActive(true);
                repeatMemory.StartWordPlayer(currentImageObjects[0].word, currentImageObjects[0].wordImage);
            }
            else
            {
                repeatMemory.gameObject.SetActive(true);
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
            repeatMemory.gameObject.SetActive(false);

            if (cardsLeft > 0)
            {
                    StartCoroutine(ShowImagesTime(2.5f));
            }
            else
            {
                for(int i = 0; i < endText.Length; i++)
                {
                    endText[i].text = score.ToString() + " tegen " + aiScore.ToString();
                }

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
                StartCoroutine(WaitForNextRepeat());
            }
        }
    }

    //just a simple wait thing
    IEnumerator WaitForNextRepeat()
    {
        yield return new WaitForSeconds(1);
        repeatMemory.StartWordAI(currentImageObjects[1].word, currentImageObjects[1].wordImage);
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

            hint.objectHighlight = new GameObject[board.transform.childCount];
            for(int i = 0; i < board.transform.childCount; i++)
            {
                hint.objectHighlight[i] = board.transform.GetChild(i).gameObject;
            }
            //see comments @Hint
            hint.ReFindColors();
        }
        else
        {
            currentImageObjects[0].DisableImage();
            currentImageObjects[1].DisableImage();
        }

        clicks = 0;

        if(!rightCards)
        {
            if (playerTurn)
            {
                gameMode.noHints = true;
                playerTurn = false;
                AITurn();
            }
            else
            {
                gameMode.noHints = false;
                gameMode.PlaySound(yourTurn);
                playerTurn = true;
            }
        }
        else
        {
            if (playerTurn)
            {
                gameMode.noHints = false;
                gameMode.PlaySound(yourTurn);
                playerTurn = true;
            }
            else
            {
                gameMode.noHints = true;
                playerTurn = false;
                AITurn();
            }
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
