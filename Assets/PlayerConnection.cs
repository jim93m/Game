using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour {

	// Use this for initialization
	void Start () {

        if (isLocalPlayer == false)
        {
            return;
        }

        if (isServer)
        {
            Debug.Log("I'm the server");
        }

        if (!isServer)
        {
            Debug.Log("I'm the client");
        }

        // Dimiourgw mia karta apo to prefab tis kartas autis 
        // Kai meta dinw gia paretn tis kartas to hand zone
        //   GameObject myCard=Instantiate(PlayerCard);
        //   myCard.transform.SetParent(GameObject.Find("Hand").transform);


        CmdSpawnMyUnit();
    }
    
    public GameObject Card;
    	
	void Update () {
		
	}

    [Command]
    void CmdSpawnMyUnit() {

        GameObject myCard= Instantiate(Card);
        

        NetworkServer.SpawnWithClientAuthority(myCard, connectionToClient);
        myCard.transform.SetParent(GameObject.Find("Hand").transform);

        //  Rpc_ClientRpcSyncItemOnce(myCard);


    }
/*
    [ClientRpc]
    public void Rpc_ClientRpcSyncItemOnce(GameObject obj)
    {
        Debug.Log(obj.transform.parent.transform);
        obj.transform.SetParent(obj.transform.parent.transform);
       
    }*/

    /*   
       [Command]
       void CmdSetParentOfCard(Transform card, Transform parentToReturnTo)
       {

           card.SetParent(parentToReturnTo);

       }
       */
    public void myconnection(Transform card , Transform parentToReturnTo)
    {

        Debug.Log("The parent of card "+ card.name+ "is droped on "+ parentToReturnTo.name);
     //   CmdSetParentOfCard(card, parentToReturnTo);

    }


}
