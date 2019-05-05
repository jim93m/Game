using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerBehavior : NetworkBehaviour
{
    public bool isLocalPlayers;
    // Start is called before the first frame update
    void Start()
    {
        if (!hasAuthority)
        {

            Debug.Log("-----------NOT LOCAL PLAYER CARD");
            GetComponent<Draggable>().enabled = false;
            isLocalPlayers = false; //A very important variable with public access that keeps the information weather the object belongs to the local player
        }
        else
        {
            Debug.Log("-----------LOCAL PLAYER CARD");
            isLocalPlayers = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
