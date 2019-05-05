using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    
    float speed = 3.5f; // Speed of the zooming action
    public GameObject mainCamera ;
    Vector3 inFrontOfCameraPosition;
    Vector3 cardsBoardPosition;
    Quaternion inFrontOfCameraRotation;
    Vector3 cardsBoardRotationEul;
    Quaternion cardsBoardRotationQua;
    public bool performingZoom; 
    bool zoomed;
      
    

    // Use this for initialization
    void Start () {

        performingZoom = false;
        zoomed = false;

    }

    public void setCameraPosition()
    {
        Debug.Log("camera seted Cardzoom script");
        

        mainCamera = GameObject.FindWithTag("MainCamera");
        inFrontOfCameraPosition = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z + 60);
        cardsBoardRotationEul = transform.rotation.eulerAngles; // Saving the current / default roatation of the object in eulerAngles and in Quaternion, as both are needed
        cardsBoardRotationQua = transform.rotation;


        if (GetComponent<MultiplayerBehavior>().isLocalPlayers == false) // If the card of the runing script is NOT local players then assign a rotation for its zooming action 
        {
            cardsBoardRotationEul = new Vector3(cardsBoardRotationEul.x, cardsBoardRotationEul.y , cardsBoardRotationEul.z + 180);
            
            inFrontOfCameraRotation = Quaternion.Euler(cardsBoardRotationEul);
                       
        }
        else
        {            
            inFrontOfCameraRotation = Quaternion.Euler(cardsBoardRotationEul);
                       
        }       
        
        
    }

    public void zoomIn()
    {       
        this.transform.position = Vector3.Lerp(transform.position, inFrontOfCameraPosition, Time.deltaTime * speed);
        this.transform.rotation = Quaternion.Lerp(transform.rotation, inFrontOfCameraRotation, Time.deltaTime * speed);
        // transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime * smooth);
    }


    public void zoomOut()
    {
        this.transform.position = Vector3.Lerp(transform.position, cardsBoardPosition, Time.deltaTime * speed);
        this.transform.rotation = Quaternion.Lerp(transform.rotation, cardsBoardRotationQua, Time.deltaTime * speed);
    }


    void Update () {

        if (Input.GetMouseButtonDown(1) && mouseOver.mouseOverObject == this.gameObject && performingZoom == false)
        { //If right click is pushed and the mouse is over this card and this card is not currently zooming (in or out)

            if (zoomed == false) // If the card is not zoomed (meaning that it is on its original position), keep its position for when zoomed out is called
            {
                cardsBoardPosition = this.transform.position;
            }

            performingZoom = true;
            zoomed = !zoomed;

            this.GetComponent<Draggable>().enabled = false;    // The card is not dragabble until zoom out is called


        }

        if (performingZoom && (zoomed ==true)) {
            zoomIn();
        }
      
        if (performingZoom && (zoomed == false))
        {
            zoomOut();
        }

        if (Vector3.Distance(transform.position, inFrontOfCameraPosition) < 0.1f )
        {
            performingZoom = false;
        }

        if (Vector3.Distance(transform.position, cardsBoardPosition) < 0.1f)  // When the distance between the zooming-out card and its starting position is very small, run the clean-up routine.
        { // To be sure of the time that the card's zoom has ended, we could check it's speed. But this would lead in a some seconds delay, as it's speed at the end of the zoom is very small but not zero for 2 seconds

            performingZoom = false;
            if (GetComponent<MultiplayerBehavior>().isLocalPlayers) { // Check if the zoomed out card is local players, for not enabling draggable script to the enemy
                this.GetComponent<Draggable>().enabled = true;    // Renable the draggable script, so the card can again be druged
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(GameObject.Find("MyHand").GetComponent<RectTransform>());  // Rebuild the layout so the zoomed out card is again placed under its parent
         

        }



    }


}
