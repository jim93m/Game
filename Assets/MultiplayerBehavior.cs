using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerBehavior : NetworkBehaviour
{
    public bool isLocalPlayers;
    public string player;
    void Start()
    {
        if (!hasAuthority)
        {

            Debug.Log("-----------NOT LOCAL PLAYER CARD");
            GetComponent<Draggable>().enabled = false;
            isLocalPlayers = false; //A very important variable with public access that keeps the information weather the object belongs to the local player

            if (isServer)
            {
                player = "Client";
            }
            else
            {
                player = "Host";
            }

        }
        else
        {
            Debug.Log("-----------LOCAL PLAYER CARD");
            isLocalPlayers = true;

            if (isServer)
            {
                player = "Host";
            }
            else
            {
                player = "Client";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
