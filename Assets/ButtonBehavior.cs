using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonBehavior : MonoBehaviour
{

    public Button actionButton;
    
    void Start()
    {
        addListener();

    }

    public void addListener()
    {
        actionButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("The button have been pushed " );
        GameObject.Find("MyConnection").GetComponent<PlayerConnection>().CmdNextTurnState();


    }
}
