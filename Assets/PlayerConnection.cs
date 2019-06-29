using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerConnection : NetworkBehaviour
{

    GameObject battleScene;
    GameObject waitingForPlayersScene;
    public bool isHost;

    // Use this for initialization
    void Start()
    {

        if (isLocalPlayer == true)
        {
            name = "MyConnection";
        }
        else
        {
            //GameObject.Find("MyConnection").GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);

            name = "EnemysConnection";
            //GameObject.Find("EnemysConnection").GetComponent<PlayerConnection>().enabled = false ;
            return;
        }

        if (isLocalPlayer == false)
        {
            return;
        }

        isHost = isServer;
        battleScene = GameObject.Find("BattleScene");
        waitingForPlayersScene = GameObject.Find("WaitingForPlayersScene");
        waitingForPlayersScene.SetActive(true);
        battleScene.SetActive(false);

        currentState = TurnStates.SETUPGAME; // Turn related code
        turnNumber = 0;
        localPlayerTurn = PlayerTurn.CLIENT;
        
    }
    [ClientRpc]
    public void RpcStartGame()
    {
        if (isLocalPlayer == false)
        {
            return;
        }

        Invoke("setUpGame1", 1); // We call setUpGame with a small delay because RpcStartGame() is called right after the client joins the game we give the gameobjects some time to spwan spawned
        Invoke("setUpGame2", 2); // Calling the set up for the CardZoom script, but with some delay, just to give some time for everycard to spawn so it can be found
                                 // Invoke("setUpCards", 1);

        if (isServer)
        {
            //CmdTest();
            Invoke("setUpGame3", 1);
        }

    }

    [ClientRpc]
    public void RpcTest()
    {
        name = "jim"; //////////////////////  for testing
        if (isLocalPlayer == false)
        {
            return;
        }
        Debug.Log("Local PLayer");
    }

    [Command]
    void CmdTest()
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("PlayerObject");
        foreach (GameObject player in playerList)
        {
            player.GetComponent<PlayerConnection>().RpcTest();
        }
        
    }

        public void setUpGame3()
    {
        CmdNextTurnState(); // VERY IMPORTANT - Once everything is set up, we call CmdNextTurnState so the turn base system starts
    }
        public void setUpGame1()
    {
        
        string faction = ""; // I just had to assign an empty string so the compiler do not think that faction might be unassigned

        if (isLocalPlayer == false)
        {
            return;
        }
        
        waitingForPlayersScene.SetActive(false);  //  we change the view of the player from the waiting screen to trhe game screen
        battleScene.SetActive(true);

        Debug.Log("Activating Objects");
        // For unknown reason when the game changes scene to BattleScene, all the objects with networkid are deactivated. We have to find them and activate them
        GameObject[] networkedObjects = GameObject.FindGameObjectsWithTag("NetworkedObjects"); // Find all the networked objects. They all have the NetworkedObjects tag   
        foreach (GameObject networkedObject in networkedObjects) // For each one of them access all their children and activate them.
        {
            foreach (Transform child in networkedObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        GameObject.Find("NetworkManager").GetComponent<NetworkManagerCustom>().SetupOtherSceneButtons();


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

     //   GameObject.Find("MyConnection").GetComponent<NetworkIdentity>().RemoveClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
    //    GameObject.Find("MyConnection").GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);

    }

    public void setUpGame2()
    {
        GameObject[] cardList = GameObject.FindGameObjectsWithTag("Card"); // I'm gathering all objects taged as cards and call their CardZoom.setCameraPosition 
        Debug.Log("The available card: " + cardList.Length);
        foreach (GameObject card in cardList)
        {
            card.GetComponent<CardZoom>().setCameraPosition();
        }

    }

    public void setUpCards1()
    {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card"); // For all cards in the game, disable their draggable component at the end of a players turn
        foreach (GameObject card in cards)
        {
            card.GetComponent<Draggable>().enabled = false;
        }
        GameObject.Find("ActionButton").GetComponent<Button>().onClick.RemoveAllListeners();
        if (isServer)
        {
            CmdNextTurnState(); // Start the game by changing turnState to STARTTURN
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
    void CmdSpawnMyUnit(string faction, NetworkInstanceId handNetId)
    {

        GameObject myCard;
        if (faction.Equals("Romans"))  // Deppending the faction server will spawn the right cards
        {
            myCard = Instantiate(Card);
        }
        else if (faction.Equals("Celts"))
        {
            myCard = Instantiate(Card2);
        }
        else { myCard = Instantiate(Card2); } // Does not going to happen. I just had to put an else clause so the compiler do not think that myCard might be unassigned

        GameObject hand = ClientScene.FindLocalObject(handNetId);  // Find the hand localy through netid and as the script runs on the host, if it's the clients (EnemyHand), rotate the spawned card
        if (hand == GameObject.Find("MyHand"))
        {
            Vector3 cardsEulAng = myCard.transform.eulerAngles; // We keep the cards angles
            myCard.transform.eulerAngles = new Vector3(cardsEulAng.x, cardsEulAng.y, cardsEulAng.z + 180); // We assign to the objects rotation, a new vector
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

    



    // Turn State Machine|


    public enum TurnStates  // All possible turn states
    {   
        SETUPGAME,
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

   

    [SyncVar]
    public TurnStates currentState;
    //[SyncVar]
    public int turnNumber;
   // [SyncVar]
   // public PlayerTurn playerTurn;
    public PlayerTurn localPlayerTurn;

    [Command]
    public void CmdNextTurnState()
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("PlayerObject");
        
        if (currentState == TurnStates.SETUPGAME)
        {
            //localPlayerTurn = playerTurn;
            foreach (GameObject player in playerList)
            {
                player.GetComponent<PlayerConnection>().RpcSetUpGame(localPlayerTurn);
            }
            

        }

        if (currentState == TurnStates.STARTTURN)
        {
            

            if (localPlayerTurn == PlayerTurn.CLIENT) // Change players turn, toggling between host and client
            {
                localPlayerTurn = PlayerTurn.HOST;
            }
            else if (localPlayerTurn == PlayerTurn.HOST)
            {
                localPlayerTurn = PlayerTurn.CLIENT;
            }
            
            foreach (GameObject player in playerList)
            {
                player.GetComponent<PlayerConnection>().RpcChangeTurnStateStart(localPlayerTurn);
            }
            

        }
        else if (currentState == TurnStates.PLAYERACTIONS1)
        {
            foreach (GameObject player in playerList)
            {
                player.GetComponent<PlayerConnection>().RpcChangeTurnStatePlayerActions1();
            }
            

        }
        else if (currentState == TurnStates.BATTLE)
        {
            foreach (GameObject player in playerList)
            {
                player.GetComponent<PlayerConnection>().RpcChangeTurnStateBattle();
            }
            

        }
        else if (currentState == TurnStates.PLAYERACTIONS2)
        {
            foreach (GameObject player in playerList)
            {
                player.GetComponent<PlayerConnection>().RpcChangeTurnStatePlayerActions2();
            }
            

        }
        else if (currentState == TurnStates.ENDTURN)
        {
            foreach (GameObject player in playerList)
            {
                player.GetComponent<PlayerConnection>().RpcChangeTurnStateEndTurn();
            }                      

        }

    }

    [Command]
    public void CmdSetTurnState(TurnStates curState)
    {
         currentState = curState;
    }

    [ClientRpc]
    public void RpcSetUpGame(PlayerTurn locPlTurn)
    {
        if (isLocalPlayer == false)
        {
            return;
        }
        localPlayerTurn = locPlTurn;

        GameObject.Find("ActionButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ActionButton").GetComponent<Button>().interactable = false;
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card"); // For all cards in the game, disable their draggable component at the end of a players turn
        foreach (GameObject card in cards)
        {
            card.GetComponent<Draggable>().enabled = false;
        }
        // From this point and on, the code will be executed only on the player that plays the first turn
        /*
        if (isServer == true && localPlayerTurn == PlayerTurn.CLIENT)
        { return; }
        if (isServer == false && localPlayerTurn == PlayerTurn.HOST)
        { return; }
        */

        if (isServer == true && turnNumber % 2 == 1)
        {
            return;
        }
        if (isServer == false && turnNumber % 2 == 0)
        {
            return;
        }

        CmdSetTurnState(TurnStates.STARTTURN);
        CmdNextTurnState();
    }

    public string playersTurn() {

        if (turnNumber % 2 == 1) // If the number of the turn is odd the it's clients turn
        {
            return "Client";
        }
        else
        {
            return "Host";  // If the number of the turn is odd the it's clients turn
        }
    }
    [ClientRpc]
    public void RpcChangeTurnStateStart(PlayerTurn locPlTurn)
    {
        
         if (isLocalPlayer == false)
        {
            return;
        }

        localPlayerTurn = locPlTurn;
        turnNumber++; 

        GameObject.Find("RoundNo").GetComponent<TextMeshProUGUI>().text = turnNumber.ToString();
        GameObject.Find("PlayerText").GetComponent<TextMeshProUGUI>().text = playersTurn();
        GameObject.Find("PhaseText").GetComponent<TextMeshProUGUI>().text = currentState.ToString();
        

        /*
        if (isServer == true && localPlayerTurn == PlayerTurn.CLIENT)
        { return; }
        if (isServer == false && localPlayerTurn == PlayerTurn.HOST)
        { return; }
        */

        if (isServer == true && turnNumber % 2 == 1)
        {
            return;
        }
        if (isServer == false && turnNumber % 2 == 0)
        {
            return;
        }
        GameObject.Find("ActionButtonText").GetComponent<TextMeshProUGUI>().text = "BATTLE";

        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card"); // For all cards in the game that belong to the local player, enable their draggable component, and reset it's remaining movement
        foreach (GameObject card in cards)
        {
            if (card.GetComponent<MultiplayerBehavior>().isLocalPlayers)
            {
                card.GetComponent<Draggable>().enabled = true;
                card.transform.GetChild(0).GetComponent<CardViz>().resetRemainingMovement();
            }
        }
        GameObject.Find("ActionButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ActionButton").GetComponent<ButtonBehavior>().addListener(); // Also enable the ButtonBehavior script of the ActionButton
        GameObject.Find("ActionButton").GetComponent<Button>().interactable = true;

        Debug.Log("RpcChangeTurnStateStart has been called");

        
        CmdSetTurnState(TurnStates.PLAYERACTIONS1);

        CmdNextTurnState();
    }

    [ClientRpc]
    public void RpcChangeTurnStatePlayerActions1()
    {
        if (isLocalPlayer == false)
        {
            return;
        }

        GameObject.Find("RoundNo").GetComponent<TextMeshProUGUI>().text = turnNumber.ToString();
        GameObject.Find("PlayerText").GetComponent<TextMeshProUGUI>().text = playersTurn();
        GameObject.Find("PhaseText").GetComponent<TextMeshProUGUI>().text = currentState.ToString();

        /*
        if (isServer == true && localPlayerTurn == PlayerTurn.CLIENT)
        { return; }
        if (isServer == false && localPlayerTurn == PlayerTurn.HOST)
        { return; }
        */

        if (isServer == true && turnNumber % 2 == 1){
            return;
        }
        if (isServer == false && turnNumber % 2 == 0){
            return;
        }

        Debug.Log(" RpcChangeTurnStatePlayerActions1 has been called");
        CmdSetTurnState(TurnStates.BATTLE);

    }

    [ClientRpc]
    public void RpcChangeTurnStateBattle()
    {
       if (isLocalPlayer == false)
        {
            return;
        }

        GameObject.Find("RoundNo").GetComponent<TextMeshProUGUI>().text = turnNumber.ToString();
        GameObject.Find("PlayerText").GetComponent<TextMeshProUGUI>().text = playersTurn();
        GameObject.Find("PhaseText").GetComponent<TextMeshProUGUI>().text = currentState.ToString();
        

        /*
        if (isServer == true && localPlayerTurn == PlayerTurn.CLIENT)
        { return; }
        if (isServer == false && localPlayerTurn == PlayerTurn.HOST)
        { return; }
        */

        if (isServer == true && turnNumber % 2 == 1)
        {
            return;
        }
        if (isServer == false && turnNumber % 2 == 0)
        {
            return;
        }

        Debug.Log(" RpcChangeTurnStateBattle has been called");
        CmdCalculateBattle();
        CmdSetTurnState(TurnStates.PLAYERACTIONS2);

        CmdNextTurnState();
    }

    [ClientRpc]
    public void RpcChangeTurnStatePlayerActions2()
    {
        if (isLocalPlayer == false)
        {
            return;
        }

        GameObject.Find("RoundNo").GetComponent<TextMeshProUGUI>().text = turnNumber.ToString();
        GameObject.Find("PlayerText").GetComponent<TextMeshProUGUI>().text = playersTurn();
        GameObject.Find("PhaseText").GetComponent<TextMeshProUGUI>().text = currentState.ToString();
        
        /*
        if (isServer == true && localPlayerTurn == PlayerTurn.CLIENT)
        { return; }
        if (isServer == false && localPlayerTurn == PlayerTurn.HOST)
        { return; }
        */

        if (isServer == true && turnNumber % 2 == 1)
        {
            return;
        }
        if (isServer == false && turnNumber % 2 == 0)
        {
            return;
        }

        GameObject.Find("ActionButtonText").GetComponent<TextMeshProUGUI>().text = "END TURN";

        Debug.Log("RpcChangeTurnStatePlayerActions2 has been called");
        CmdSetTurnState(TurnStates.ENDTURN);

    }
    [ClientRpc]
    public void RpcChangeTurnStateEndTurn()
    {
        if (isLocalPlayer == false)
        {
            return;
        }

        GameObject.Find("RoundNo").GetComponent<TextMeshProUGUI>().text = turnNumber.ToString();
        GameObject.Find("PlayerText").GetComponent<TextMeshProUGUI>().text = playersTurn();
        GameObject.Find("PhaseText").GetComponent<TextMeshProUGUI>().text = currentState.ToString();

        /*
        if (isServer == true && localPlayerTurn == PlayerTurn.CLIENT)
        { return; }
        if (isServer == false && localPlayerTurn == PlayerTurn.HOST)
        { return; }
        */

        if (isServer == true && turnNumber % 2 == 1)
        {
            return;
        }
        if (isServer == false && turnNumber % 2 == 0)
        {
            return;
        }

        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card"); // For all cards in the game, disable their draggable component at the end of a players turn
        foreach (GameObject card in cards)
        {
            card.GetComponent<Draggable>().enabled = false;
        }
        GameObject.Find("ActionButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ActionButton").GetComponent<Button>().interactable = false;


        Debug.Log("RpcChangeTurnStateEndTurn has been called");
        CmdSetTurnState(TurnStates.STARTTURN);

        CmdNextTurnState();
    }

    public class Coordinates
    {
        public int x;
        public int y;
        public bool equals(Coordinates coordinates)
        {
            if (x == coordinates.x && y == coordinates.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class EngagedCouple
    {
        public GameObject dropzoneA;
        public GameObject dropzoneB;
        public EngagedCouple(GameObject a, GameObject b)
        {
            dropzoneA = a;
            dropzoneB = b;
        }
    }

    [Command]
    public void CmdCalculateBattle()
    {
        IList<EngagedCouple> engagedDropzoneList = findEngagedDropzones();
        executeFighting(engagedDropzoneList);
        cleanUpRetreatedCards();
    }

    public IList<EngagedCouple> findEngagedDropzones()
    {
        IList<Coordinates> coordinateList = new List<Coordinates>();
        IList<GameObject> occupiedDropzoneList = new List<GameObject>();
        IList<EngagedCouple> engagedDropzoneList = new List<EngagedCouple>();

        GameObject[] dropzones = GameObject.FindGameObjectsWithTag("DropZone");
        foreach (GameObject dropzone in dropzones)
        {
            if (dropzone.transform.childCount > 0) // Checking if the current dropzone is occupied. Anything we plan to do considers only the occupied dropzones
            {
                Coordinates coordinate = new Coordinates();
                coordinate.x = dropzone.GetComponent<BattleDropzone>().idX;
                coordinate.y = dropzone.GetComponent<BattleDropzone>().idY;
                coordinateList.Add(coordinate);

                occupiedDropzoneList.Add(dropzone);

                GameObject dropzoneBellow = GameObject.Find("Tabletop " + (coordinate.y - 1).ToString() + coordinate.x.ToString()); // Attempt to find the dropzone bellow the current dropzone
                if (dropzoneBellow != null)
                { // Check to see if it is found. We do this as the dropzones of the first row, won't be having any dropzones bellow
                    if (dropzoneBellow.transform.childCount > 0) // if the dropzone bellow is also occupied
                    {
                        // Here we have also to check if the 2 cards belong to enemy players
                        EngagedCouple engagedCouple = new EngagedCouple(dropzone, dropzoneBellow);
                        engagedDropzoneList.Add(engagedCouple);  // Save the dropzones containing engaged cards

                    }
                }
            }

        }
        return engagedDropzoneList;
    }

    public void executeFighting(IList<EngagedCouple> engagedDropzoneList)
    {
        foreach (EngagedCouple engagedCouple in engagedDropzoneList)  // For each engaged couple
        {
            GameObject cardA = engagedCouple.dropzoneA.transform.GetChild(0).gameObject;
            GameObject cardB = engagedCouple.dropzoneB.transform.GetChild(0).gameObject;
            // We access their stats
            int attackA = cardA.transform.GetChild(0).GetComponent<CardViz>().getAttack();
            int defenceA = cardA.transform.GetChild(0).GetComponent<CardViz>().getDefense();
            int armorA = cardA.transform.GetChild(0).GetComponent<CardViz>().getArmor();
            int moraleA = cardA.transform.GetChild(0).GetComponent<CardViz>().getMorale();

            int attackB = cardB.transform.GetChild(0).GetComponent<CardViz>().getAttack();
            int defenceB = cardB.transform.GetChild(0).GetComponent<CardViz>().getDefense();
            int armorB = cardB.transform.GetChild(0).GetComponent<CardViz>().getArmor();
            int moraleB = cardB.transform.GetChild(0).GetComponent<CardViz>().getMorale();


            int damageDealtToB = 0;  // damageDealtToB is going to hold the total damage that will be dealt to card B
            int damageDealtToA = 0;
            for (int i = 0; i < attackA; i++)  // We create random numbers (0-6) as a dice trow, times the attack stat of the card
            {
                int attackAttempt = Random.Range(1, 6);
                if (attackAttempt >= (defenceB + armorB))  // And then for each dice that's equal or higher to the sum of defence and armor stats, we add 1 damage to be assign to the other card
                {
                    damageDealtToB++;
                }
            }
            moraleB = moraleB - damageDealtToB;

            for (int i = 0; i < attackB; i++)
            {
                int attackAttempt = Random.Range(1, 6);
                if (attackAttempt >= (defenceA + armorA))
                {
                    damageDealtToA++;
                }
            }
            moraleA = moraleA - damageDealtToA;

            cardA.transform.GetChild(0).GetComponent<CardViz>().setMorale(moraleA);   // ! At this point we assign localy the new morales of the cards, as althought we send an rpc that will also be recieved from the server,
            cardB.transform.GetChild(0).GetComponent<CardViz>().setMorale(moraleB);   //  it will happen with a delay, as it's networked, and until then the server will have already access the morales yet again, and won't
                                                                                      //  have the updated values
            GameObject[] playerList = GameObject.FindGameObjectsWithTag("PlayerObject");
            foreach (GameObject player in playerList)
            {
                player.GetComponent<PlayerConnection>().RpcSyncCard(cardA.name, moraleA);  // We send the rpcs to update the values of the cards
                player.GetComponent<PlayerConnection>().RpcSyncCard(cardB.name, moraleB);
            }
        }

    }

        public void cleanUpRetreatedCards()
    {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cards)
        {
            if (card.transform.GetChild(0).GetComponent<CardViz>().getMorale() <= 0)  // For each card we check if it has a morale <= 0, to remove it from the game
            {
                GameObject[] playerList = GameObject.FindGameObjectsWithTag("PlayerObject");
                foreach (GameObject player in playerList)
                {
                    player.GetComponent<PlayerConnection>().RpcCardRemoveFromGame(card.name);

                }

            }
        }
    }
    [ClientRpc]
    public void RpcSyncCard(string cardName, int morale)  // Updating the card values on the clients
    {
        GameObject card = GameObject.Find(cardName);
        card.transform.GetChild(0).GetComponent<CardViz>().setMorale(morale);
    }
    [ClientRpc]
    public void RpcCardRemoveFromGame(string cardName)  // Removing a card from the battlefield
    {
        GameObject card = GameObject.Find(cardName);
        card.transform.SetParent(GameObject.Find("OffGameSpace").transform);
    }



}
