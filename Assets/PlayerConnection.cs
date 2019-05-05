using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerConnection : NetworkBehaviour {

    GameObject battleScene;
    GameObject waitingForPlayersScene;
    public bool isHost; 

    // Use this for initialization
    void Start () {

        if (isLocalPlayer == true)
        {
            name = "MyConnection";
        }
        else
        {
            name = "EnemysConnection";
            return;
        }

        isHost = isServer;
        battleScene = GameObject.Find("BattleScene");
        waitingForPlayersScene = GameObject.Find("WaitingForPlayersScene");
        waitingForPlayersScene.SetActive(true);
        battleScene.SetActive(false);
        /* DontDestroyOnLoad(transform.gameObject);

         // Create a temporary reference to the current scene.
         Scene currentScene = SceneManager.GetActiveScene();

         // If the player has been created inside PreBattle scene, then just return. Actualy we dont even want the player to be spawn at this scene, but anyway.
         if (currentScene.name == "PreBattle")
         {
             return;
         }

         setUpGame();

     */
    }
    [ClientRpc]
    public void RpcStartGame()
    {
        Invoke("setUpGame", 2); // We call setUpGame with a small delay because RpcStartGame() is called right after the client joins the game we give the gameobjects some time to spwan spawned
        
    }
    public void setUpGame()
    {
        string faction=""; // I just had to assign an empty string so the compiler do not think that faction might be unassigned

        if (isLocalPlayer == false)
        {
            return;
        }

        waitingForPlayersScene.SetActive(false);  //  we change the view of the player from the waiting screen to trhe game screen
        battleScene.SetActive(true);

        // For unknown reason when the game changes scene to BattleScene, all the objects with networkid are deactivated. We have to find them and activate them
        GameObject[] networkedObjects = GameObject.FindGameObjectsWithTag("NetworkedObjects"); // Find all the networked objects. They all have the NetworkedObjects tag   
        foreach (GameObject networkedObject in networkedObjects) // For each one of them access all their children and activate them.
        {           
            foreach (Transform child in networkedObject.transform)
            {                
                child.gameObject.SetActive(true);
            }
        }

        

        if (isServer)
        {
            
            GameObject.Find("HostCamera").tag = "MainCamera";// Assigning the two different cameras for each player
            GameObject.Find("HostCamera").SetActive(true);
            GameObject.Find("ClientCamera").SetActive(false);

            GameObject.Find("HostHandPanel").transform.Find("Hand").name = "MyHand"; // Naming localy the players and the enemys hand
            GameObject.Find("ClientHandPanel").transform.Find("Hand").name = "EnemyHand";

            Debug.Log("I'm the server");

            faction = "Romans"; // Setting the deck that is going to be spawned
        }

        if (!isServer)
        {
            GameObject.Find("ClientCamera").tag = "MainCamera";
            GameObject.Find("ClientCamera").SetActive(true);
            GameObject.Find("HostCamera").SetActive(false);

            GameObject.Find("ClientHandPanel").transform.Find("Hand").name = "MyHand";
            GameObject.Find("HostHandPanel").transform.Find("Hand").name = "EnemyHand";

            Debug.Log("I'm the client");

            faction = "Celts";
        }
        NetworkInstanceId handNetId = GameObject.Find("MyHand").GetComponent<NetworkIdentity>().netId;
        CmdSpawnMyUnit(faction, handNetId);

        Invoke("setUpZoom", 1); // Calling the set up for the CardZoom script, but with some delay, just to give some time for everycard to spawn so it can be found

    }

    public void setUpZoom()
    {
        GameObject[] cardList = GameObject.FindGameObjectsWithTag("Card"); // I'm gathering all objects taged as cards and call their CardZoom.setCameraPosition 
        Debug.Log("The available card: " + cardList.Length);
        foreach (GameObject card in cardList)
        {
            card.GetComponent<CardZoom>().setCameraPosition();
        }
    }

    void OnEnable() // I have to implement the all the following methonds. It does not let me implement only OnSceneLoaded that I need
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 2)
        {            
          //  setUpGame(); // When the second scene loads setup the game
        }
    }

    // The server uses a Rpc call in order to tell the clients to change their scenes. This is called while clients are ready to load the BattleScene
    [ClientRpc]
    public void RpcChangeScene(int n)
    {
        SceneManager.LoadScene(n);        
    }

    
    public GameObject Card;
    public GameObject Card2;


    [Command]
    void CmdSpawnMyUnit(string faction, NetworkInstanceId handNetId) {

        GameObject myCard;
        if (faction.Equals("Romans"))  // Deppending the faction server will spawn the right cards
        {
            myCard = Instantiate(Card);
        }
        else if (faction.Equals("Celts"))
        {
            myCard = Instantiate(Card2);
        }
        else{ myCard = Instantiate(Card2); } // Does not going to happen. I just had to put an else clause so the compiler do not think that myCard might be unassigned

        GameObject hand = ClientScene.FindLocalObject(handNetId);  // Find the hand localy through netid and as the script runs on the host, if it's the clients (EnemyHand), rotate the spawned card
        if (hand == GameObject.Find("MyHand"))
        {
            Vector3 cardsEulAng = myCard.transform.eulerAngles; // We keep the cards angles
            myCard.transform.eulerAngles = new Vector3(cardsEulAng.x, cardsEulAng.y , cardsEulAng.z + 180); // We assign to the objects rotation, a new vector
        }


        NetworkServer.SpawnWithClientAuthority(myCard, connectionToClient);
        //myCard.transform.SetParent(GameObject.Find("MyHand").transform);  

        NetworkInstanceId cardNetId = myCard.GetComponent<NetworkIdentity>().netId;
        //NetworkInstanceId handNetId = GameObject.Find("MyHand").GetComponent<NetworkIdentity>().netId;
              
        RpcSetParent(cardNetId, handNetId);  // After the server spawn the card, it calls the parenting method on all clients giving as attributes, their netids
        
    }

    public void setParent(Transform card, Transform parentToReturnTo) // A local object calls his local MyConnection.setParent to ask to parent it
    {

        NetworkInstanceId cardNetId = card.gameObject.GetComponent<NetworkIdentity>().netId;
        NetworkInstanceId handNetId = parentToReturnTo.gameObject.GetComponent<NetworkIdentity>().netId;
        CmdSetParent(cardNetId, handNetId); // After getting the netids of the attributes the method asks the server to command everyone (it's self included) to parent the given objects

    }

    [Command]
    void CmdSetParent(NetworkInstanceId childNetId, NetworkInstanceId parentNetId) // The server then just asks everyone to execute the parenting of the objects locally
    {
        RpcSetParent(childNetId, parentNetId);
    }

    [ClientRpc]
    public void RpcSetParent(NetworkInstanceId childNetId, NetworkInstanceId parentNetId) // Here happens the actual local parenting on every players pc
    {
        
        GameObject child = ClientScene.FindLocalObject(childNetId);
        GameObject parent = ClientScene.FindLocalObject(parentNetId);
       
        child.transform.SetParent(parent.transform);

    }
   


}
