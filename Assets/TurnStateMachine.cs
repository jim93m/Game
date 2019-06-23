using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class TurnStateMachine : NetworkBehaviour
{/*
    public enum TurnStates {
        STARTTURN,
        PLAYERACTIONS1,
        BATTLE,
        PLAYERACTIONS2,
        ENDTURN,
        LOSE,
        WIN
    }

    public enum PlayerTurn
    {
        HOST,
        CLIENT
    }

    void Start()
    {
        currentState = TurnStates.STARTTURN;
        turnNumber = 1;
        playerTurn = PlayerTurn.HOST;
    }

    [SyncVar]
    public TurnStates currentState; 
    [SyncVar]
    public int turnNumber;
    [SyncVar]
    public PlayerTurn playerTurn;

    [Command]
    public void CmdNextTurnState()
    {
       

        if (currentState == TurnStates.STARTTURN) 
        {
            if (playerTurn == PlayerTurn.CLIENT) // Change players turn, toggling between host and client
            {
                playerTurn = PlayerTurn.HOST;
            }
            else if (playerTurn == PlayerTurn.HOST)
            {
                playerTurn = PlayerTurn.CLIENT;
            }

            RpcChangeTurnStateStart();
            
        }
        else if (currentState == TurnStates.PLAYERACTIONS1)
        {
            
            RpcChangeTurnStatePlayerActions1();
            
        }
        else if (currentState == TurnStates.BATTLE)
        {
            
            RpcChangeTurnStateBattle();
            
        }
        else if (currentState == TurnStates.PLAYERACTIONS2)
        {
            
            RpcChangeTurnStatePlayerActions2();
            
        }
        else if (currentState == TurnStates.ENDTURN)
        {
            
            RpcChangeTurnStateEndTurn();
            turnNumber++;

        }
        
    }

    [Command]
    public void CmdSetTurnState(TurnStates curState)
    {
        currentState = curState;
    }

    [ClientRpc]
    public void RpcChangeTurnStateStart()
    {
        
        Debug.Log("The turn is for player " + playerTurn);

        if (isServer == true && playerTurn == PlayerTurn.CLIENT)
        { return; }
        if (isServer == false && playerTurn == PlayerTurn.HOST)
        { return; }

        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card"); // For all cards in the game that belong to the local player, enable their draggable component 
        foreach (GameObject card in cards)
        {
            if (card.GetComponent<MultiplayerBehavior>().isLocalPlayers)
            {
                card.GetComponent<Draggable>().enabled = true;  
            }            
        }
        GameObject.Find("ActionButton").GetComponent<ButtonBehavior>().addListener(); // Also enable the ButtonBehavior script of the ActionButton
        
        Debug.Log("The turn stage has changed to " + currentState);

        GameObject.Find("RoundNo").GetComponent<TextMeshProUGUI>().text = turnNumber.ToString();
        CmdSetTurnState(TurnStates.PLAYERACTIONS1);

        CmdNextTurnState();
    }

    [ClientRpc]
    public void RpcChangeTurnStatePlayerActions1()
    {
        if (isServer == true && playerTurn == PlayerTurn.CLIENT)
        { return; }
        if (isServer == false && playerTurn == PlayerTurn.HOST)
        { return; }


        Debug.Log("The turn stage has changed to " + currentState);
        CmdSetTurnState(TurnStates.BATTLE);

    }

    [ClientRpc]
    public void RpcChangeTurnStateBattle()
    {
        if (isServer == true && playerTurn == PlayerTurn.CLIENT)
        { return; }
        if (isServer == false && playerTurn == PlayerTurn.HOST)
        { return; }


        Debug.Log("The turn stage has changed to " + currentState);
        CmdSetTurnState(TurnStates.PLAYERACTIONS2);

        CmdNextTurnState();
    }

    [ClientRpc]
    public void RpcChangeTurnStatePlayerActions2()
    {
        if (isServer == true && playerTurn == PlayerTurn.CLIENT)
        { return; }
        if (isServer == false && playerTurn == PlayerTurn.HOST)
        { return; }


        Debug.Log("The turn stage has changed to " + currentState);
        CmdSetTurnState(TurnStates.ENDTURN);

    }
    [ClientRpc]
    public void RpcChangeTurnStateEndTurn()
    {
        if (isServer == true && playerTurn == PlayerTurn.CLIENT)
        { return; }
        if (isServer == false && playerTurn == PlayerTurn.HOST)
        { return; }


        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card"); // For all cards in the game, disable their draggable component at the end of a players turn
        foreach (GameObject card in cards) 
        {
            card.GetComponent<Draggable>().enabled = false;
        }
        GameObject.Find("ActionButton").GetComponent<Button>().onClick.RemoveAllListeners();  

        Debug.Log("The turn stage has changed to " + currentState);
        CmdSetTurnState(TurnStates.STARTTURN);

        CmdNextTurnState();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    */
}
