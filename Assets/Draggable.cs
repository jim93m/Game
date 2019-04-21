using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

{
    private Vector3 mOffset;
    private float mZCoord;

    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;
    public GameObject PlaceholderCard;
    GameObject placeholder; // On the editor we assign to this object variable the PlaceholderCard 
    public List<GameObject> listOfObjectsToHilight = new List<GameObject>();
    Color defaultColorOfCardGrid;
    int draggedObjectParentRow;
    int draggedObjectParentColumn;
    public Vector3 dropPosition;

    void Start()
    {
        defaultColorOfCardGrid = GameObject.Find("Tabletop 11").transform.GetComponent<Image>().color; // Keep the default color of the card before anything change it

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // Store offset = gameobject world pos - mouse world pos

        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

        hightlightObjectsForValidMove(); // Find the valid dropzones for a card to move into and hightlight them

        placeholder = Instantiate(PlaceholderCard);
        placeholder.transform.SetParent(this.transform.parent);  // Create a placeholder and set for it's parent the druged card's parent. I do this so an invisible card keeps the physical place of the druged one until the drug ends

        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void hightlightObjectsForValidMove()  // Find the valid dropzones for a card to move into and hightlight them
    {
        if (this.transform.parent.gameObject == GameObject.Find("Hand"))  // If the parent of the druged card is the hand
        {
            for (int i = 1; i <= 7; i++) // For all the columns at the first row, add them in the highlight list and highlight them. This is done in order to show the player the possible dropzones when moving from hand
            {
                listOfObjectsToHilight.Add(GameObject.Find("Tabletop 1" + i));
                //GameObject.Find("Tabletop 1" + i).transform.GetComponent<Image>().color = new Color32(153, 255, 153, 150);
            }
        }
        else if (this.transform.parent.gameObject != GameObject.Find("Hand")) // If the parent of the druged card is not the hand
        {
            draggedObjectParentRow = Int32.Parse(this.transform.parent.name[9].ToString()); //Then keep at which row and column is located
            draggedObjectParentColumn = Int32.Parse(this.transform.parent.name[10].ToString());

            if (GameObject.Find("Tabletop " + (draggedObjectParentRow - 1).ToString() + (draggedObjectParentColumn - 1).ToString()) != null)
            {
                listOfObjectsToHilight.Add(GameObject.Find("Tabletop " + (draggedObjectParentRow - 1).ToString() + (draggedObjectParentColumn - 1).ToString()));
            }
            if (GameObject.Find("Tabletop " + (draggedObjectParentRow - 1).ToString() + (draggedObjectParentColumn).ToString()) != null)
            {
                listOfObjectsToHilight.Add(GameObject.Find("Tabletop " + (draggedObjectParentRow - 1).ToString() + (draggedObjectParentColumn).ToString()));
            }
            if (GameObject.Find("Tabletop " + (draggedObjectParentRow - 1).ToString() + (draggedObjectParentColumn + 1).ToString()) != null)
            {
                listOfObjectsToHilight.Add(GameObject.Find("Tabletop " + (draggedObjectParentRow - 1).ToString() + (draggedObjectParentColumn + 1).ToString()));
            }
            if (GameObject.Find("Tabletop " + (draggedObjectParentRow).ToString() + (draggedObjectParentColumn - 1).ToString()) != null)
            {
                listOfObjectsToHilight.Add(GameObject.Find("Tabletop " + (draggedObjectParentRow).ToString() + (draggedObjectParentColumn - 1).ToString()));
            }
            if (GameObject.Find("Tabletop " + (draggedObjectParentRow).ToString() + (draggedObjectParentColumn + 1).ToString()) != null)
            {
                listOfObjectsToHilight.Add(GameObject.Find("Tabletop " + (draggedObjectParentRow).ToString() + (draggedObjectParentColumn + 1).ToString()));
            }
            if (GameObject.Find("Tabletop " + (draggedObjectParentRow + 1).ToString() + (draggedObjectParentColumn - 1).ToString()) != null)
            {
                listOfObjectsToHilight.Add(GameObject.Find("Tabletop " + (draggedObjectParentRow + 1).ToString() + (draggedObjectParentColumn - 1).ToString()));
            }
            if (GameObject.Find("Tabletop " + (draggedObjectParentRow + 1).ToString() + (draggedObjectParentColumn).ToString()) != null)
            {
                listOfObjectsToHilight.Add(GameObject.Find("Tabletop " + (draggedObjectParentRow + 1).ToString() + (draggedObjectParentColumn).ToString()));
            }
            if (GameObject.Find("Tabletop " + (draggedObjectParentRow + 1).ToString() + (draggedObjectParentColumn + 1).ToString()) != null)
            {
                listOfObjectsToHilight.Add(GameObject.Find("Tabletop " + (draggedObjectParentRow + 1).ToString() + (draggedObjectParentColumn + 1).ToString()));
            }

            
        }
        foreach (var theGameObject in listOfObjectsToHilight) // Loop through all objects in highlight list and highlight them
        {
            theGameObject.transform.GetComponent<Image>().color = new Color32(153, 255, 153, 150);
        }

    }



    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;  // Coordinates of mouse on the screen

        mousePoint.z = mZCoord; // Z Coordinate of the object         

        return Camera.main.ScreenToWorldPoint(mousePoint);  // Convert it to world points
    }



    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMouseAsWorldPoint() + mOffset; //Move the object at the corrected position

        

        int newSiblingIndex = placeholderParent.childCount; // The following code does the swapping when a druged card passes over the other cards in the hand
        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }
                break;
            }
        }


        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        foreach (var theGameObject in listOfObjectsToHilight) // Loop through all objects in highlight list and highlight them
        {
            Debug.Log("####   "+ theGameObject);
        }

        this.transform.SetParent(parentToReturnTo); // "Drops" the card on a certain drop zone, which is her current parent. 
        GameObject.Find("PlayerObject(Clone)").GetComponent<PlayerConnection>().myconnection(this.transform, parentToReturnTo);  // Try todo it throught the net
        
        // Clean up phase
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeholder); // Delete the placeholder on the hand

        foreach (var theGameObject in listOfObjectsToHilight) // Loop through all highlighted ogjects
        {
            theGameObject.transform.GetComponent<Image>().color = defaultColorOfCardGrid;  // For each object highlighted during the drag, set back the default color

        }
        listOfObjectsToHilight.Clear();
        draggedObjectParentRow = 0;
        draggedObjectParentColumn = 0;
      
    }

}