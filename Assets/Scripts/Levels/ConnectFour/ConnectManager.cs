using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectManager : MonoBehaviour
{
    public GameObject playerTurn;
    public GameObject AITurn;

    public static ConnectManager connectManager;

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
    void Start()
    {
        playerTurn.SetActive(true);
        AITurn.SetActive(false);
    }

    public void SwitchTurn()
    {
        //switch turns between player and AI
        //TODO: add option for extra real player
        if (playerTurn.activeSelf)
        {
            playerTurn.SetActive(false);
            AITurn.SetActive(true);
        }
        else
        {
            playerTurn.SetActive(true);
            AITurn.SetActive(false);
        }
    }
}
