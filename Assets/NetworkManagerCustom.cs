using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManagerCustom : NetworkManager
{
    public void StartupHost()
    {        
        SetPort();
        NetworkManager.singleton.StartHost();
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
        else
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
