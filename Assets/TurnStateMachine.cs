using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnStateMachine : NetworkBehaviour
{
    public enum TurnStates {
        STARTTURN,
        PLAYERACTIONS1,
        BATTLE,
        PLAYERACTIONS2,
        ENDTURN,
        LOSE,
        WIN
    }

    void Start()
    {
        currentState = TurnStates.STARTTURN;
        turnNumber = 1;
    }

    [SyncVar]
    TurnStates currentState;
    [SyncVar]
    int turnNumber;

    [Command]
    public void CmdNextTurnState()
    {
        if (currentState == TurnStates.STARTTURN)
        {
            currentState = TurnStates.PLAYERACTIONS1;
        }
        else if (currentState == TurnStates.PLAYERACTIONS1)
        {
            currentState = TurnStates.BATTLE;
        }
        else if (currentState == TurnStates.BATTLE)
        {
            currentState = TurnStates.PLAYERACTIONS2;
        }
        else if (currentState == TurnStates.PLAYERACTIONS2)
        {
            currentState = TurnStates.ENDTURN;
        }
        else if (currentState == TurnStates.ENDTURN)
        {


            currentState = TurnStates.STARTTURN;
        }
        Debug.Log("The current turn state is " + currentState);
    }
    [Command]
    public void CmdNextTurn()
    {
        turnNumber = turnNumber + 1;
    }



    

    // Update is called once per frame
    void Update()
    {
        
    }
}
