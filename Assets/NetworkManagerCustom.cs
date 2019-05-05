using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManagerCustom : NetworkManager
{
    int numOfConnectedPlayers;
    GameObject playerObject;
    GameObject battleScene;
    GameObject waitingForPlayersScene;

    void Start()
    {
        numOfConnectedPlayers = 0;
    }

        public void StartupHost()
    {        
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    // When a player connects the following callback method is called in order to know when second player has joint the game, so the match starts. That's the way that the server knows when someone has connected.
    public override void OnServerConnect(NetworkConnection connection) {
        Debug.Log("player connected");
        numOfConnectedPlayers++;
        Debug.Log("connected players "+ numOfConnectedPlayers);

        if (numOfConnectedPlayers == 2)
        {
            numOfConnectedPlayers = 0;
            Invoke("startGame", 1);  // Calling start game with a small delay to give some time for the spawning of the object on the client. This is needed because OnServerConnect is called right when the player has connected but before he has a chance for anything else

        }
    }

    public void startGame()
    {
        Debug.Log("waiting");
        
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("PlayerObject");

        Debug.Log("List length" + playerList.Length);
        foreach (GameObject player in playerList)
        {

            player.GetComponent<PlayerConnection>().RpcStartGame();
        }
    }


    /*
    // The method is used to change scene on both the host and the client
    void gameChangeScene()
    {
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("PlayerObject");       

        foreach (GameObject player in playerList)
        {
            player.GetComponent<PlayerConnection>().RpcChangeScene(2);
        }
    }
   */

        // That way the server knows tha a client has disconnected
    public override void OnServerDisconnect(NetworkConnection connection)
    {
                
    }

    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    void SetIPAddress()
    {
        string ipAddress = GameObject.Find("InputText").GetComponent<Text>().text;        
        NetworkManager.singleton.networkAddress = ipAddress;        
    }

    void SetPort()
    {        
        NetworkManager.singleton.networkPort = 7777;
    }

    void OnLevelWasLoaded(int level)
    {        
        if (level == 0)
        {            
            StartCoroutine(SetupMenuSceneButtons());
        }
        else if (level == 1)
        {
            
            SetupOtherSceneButtons();

            
                       
           

        }
    }

    IEnumerator SetupMenuSceneButtons()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject.Find("HostGameButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("HostGameButton").GetComponent<Button>().onClick.AddListener(StartupHost);
        
        GameObject.Find("JoinGameButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("JoinGameButton").GetComponent<Button>().onClick.AddListener(JoinGame);
    }

    void SetupOtherSceneButtons()
    {
        GameObject.Find("DisconnectButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("DisconnectButton").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }
}
