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
            GameObject.Find("HostCamera").tag = "MainCamera";            
            Debug.Log("I'm the server");

            CmdSpawnMyUnit();
        }

        if (!isServer)
        {
            GameObject.Find("ClientCamera").tag = "MainCamera";
            Debug.Log("I'm the client");

            CmdSpawnMyUnit2();
        }

        // Dimiourgw mia karta apo to prefab tis kartas autis 
        // Kai meta dinw gia paretn tis kartas to hand zone
        //   GameObject myCard=Instantiate(PlayerCard);


        
        
      //  CmdSetParent(controllerName, idNum);
       // GameObject.Find("Card").transform.SetParent(GameObject.Find("Hand").transform);
    }
    
    public GameObject Card;
    public GameObject Card2;

    void Update () {
		
	}

    [Command]
    void CmdSpawnMyUnit() {

        GameObject myCard= Instantiate(Card);


        NetworkServer.SpawnWithClientAuthority(myCard, connectionToClient);
        myCard.transform.SetParent(GameObject.Find("Hand").transform);

        NetworkInstanceId cardNetId = myCard.GetComponent<NetworkIdentity>().netId;
        NetworkInstanceId handNetId = GameObject.Find("Hand").GetComponent<NetworkIdentity>().netId;

        Debug.Log("Card netid = " + cardNetId.ToString());
        Debug.Log("Hand netid = " + handNetId.ToString());
        Debug.Log("Called RpcSetParent");


        RpcSetParent(cardNetId, handNetId);
        

    }

    [Command]
    void CmdSpawnMyUnit2()
    {

        GameObject myCard2 = Instantiate(Card2);


        NetworkServer.SpawnWithClientAuthority(myCard2, connectionToClient);
        myCard2.transform.SetParent(GameObject.Find("Hand").transform);

        NetworkInstanceId cardNetId = myCard2.GetComponent<NetworkIdentity>().netId;
        NetworkInstanceId handNetId = GameObject.Find("Hand").GetComponent<NetworkIdentity>().netId;

        Debug.Log("Card netid = " + cardNetId.ToString());
        Debug.Log("Hand netid = " + handNetId.ToString());
        Debug.Log("Called RpcSetParent");
        RpcSetParent(cardNetId, handNetId);
        

    }
    [ClientRpc]
    public void RpcSetParent(NetworkInstanceId childNetId, NetworkInstanceId parentNetId)
    {

        GameObject child = ClientScene.FindLocalObject(childNetId);
        GameObject parent = ClientScene.FindLocalObject(parentNetId);

        Debug.Log("Cards name = " + child.name);
        Debug.Log("Hands name = " + parent.name);

        child.transform.SetParent(parent.transform);

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
