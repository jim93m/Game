using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CardViz : MonoBehaviour
{
    public int attack;  // The card stats are public only to be shown editable in the editor. Every read and write upon them should happen throught setters and getters
    public int defense;
    public int armor;
    public int morale;
    public int movement;
    public int remainingMovement;
    public bool isLocalPlayers;
    public string player; 
    public bool inGame;

    public Card card;

    void Start()
    {
        this.gameObject.transform.Find("attackText").gameObject.GetComponent<UnityEngine.UI.Text>().text = attack.ToString();
        this.gameObject.transform.Find("defenseText").gameObject.GetComponent<UnityEngine.UI.Text>().text = defense.ToString();
        this.gameObject.transform.Find("armorText").gameObject.GetComponent<UnityEngine.UI.Text>().text = armor.ToString();
        this.gameObject.transform.Find("moraleText").gameObject.GetComponent<UnityEngine.UI.Text>().text = morale.ToString();
        remainingMovement = movement;
        inGame = true;

        isLocalPlayers = this.transform.parent.gameObject.GetComponent<MultiplayerBehavior>().isLocalPlayers;
        player = this.transform.parent.gameObject.GetComponent<MultiplayerBehavior>().player;


        //LoadCard(card);
    }

    public void setAttack(int value)
    {
        attack = value;
        //print("^^^^^^^^" + this.gameObject.transform.parent.gameObject.name);
        // In the following line firstly I m referencing the game object that the scripts run on. Secondly I m referencing its child with name "attackText", and then changing the value of its text component
        this.gameObject.transform.Find("attackText").gameObject.GetComponent<UnityEngine.UI.Text>().text = attack.ToString();
    }
    public void setDefense(int value)
    {
        defense = value;
        this.gameObject.transform.Find("defenseText").gameObject.GetComponent<UnityEngine.UI.Text>().text = defense.ToString();
    }
    public void setArmor(int value)
    {
        armor = value;
        this.gameObject.transform.Find("armorText").gameObject.GetComponent<UnityEngine.UI.Text>().text = armor.ToString();
    }
    public void setMorale(int value)
    {
        morale = value;
        this.gameObject.transform.Find("moraleText").gameObject.GetComponent<UnityEngine.UI.Text>().text = morale.ToString();
    }

    public void setMovement(int value)
    {
        movement = value;
    }

    public void setRemainingMovement(int value)
    {
        remainingMovement = value;
    }

    public int getAttack()
    {
        return attack;
    }
    public int getDefense()
    {
        return defense;
    }
    public int getArmor()
    {
        return armor;
    }
    public int getMorale()
    {
        return morale;
    }
    public int getMovement()
    {
        return movement;
    }
    public int getRemainingMovement()
    {
        return remainingMovement;
    }
    public void resetRemainingMovement()
    {
        remainingMovement = movement;
    }
    public void decreaseRemainingMovementBy1()
    {
        remainingMovement--;
    }

    public void LoadCard(Card c)
    {       
        if (c == null)
        { return; }

        card = c;

        attack = c.attack;
        defense = c.defense;
        armor = c.armor;
        morale = c.morale;
    }


}
