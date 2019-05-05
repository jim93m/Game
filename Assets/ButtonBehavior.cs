using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{

    public Button basicButton;
    
    void Start()
    {
        basicButton.onClick.AddListener(TaskOnClick);

    }

    void TaskOnClick()
    {
        GameObject.Find("MyConnection").GetComponent<TurnStateMachine>().CmdNextTurnState();

    }
}
