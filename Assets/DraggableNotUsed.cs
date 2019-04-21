using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
//using UnityEngine.Networking;

public class DraggableNotUsed : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler {

    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;
    GameObject placeholder = null;
    public List<GameObject> listOfObjectsToHilight = new List<GameObject>();
    Color defaultColorOfCardGrid;
    int draggedObjectParentRow;
    int draggedObjectParentColumn;
    public Vector3 dropPosition; 



    void Start()
    {
        defaultColorOfCardGrid = GameObject.Find("Tabletop 11").transform.GetComponent<Image>().color;
        dropPosition = this.transform.position;



    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        

            if (this.transform.parent.gameObject != GameObject.Find("Hand"))
            {
                draggedObjectParentRow = Int32.Parse(this.transform.parent.name[9].ToString());
                Debug.Log("!!!!!!!!!!!!!To draggedObjectParentRow einai " + draggedObjectParentRow);
                draggedObjectParentColumn = Int32.Parse(this.transform.parent.name[10].ToString());
                Debug.Log("!!!!!!!!!!!!!To draggedObjectParentRow einai " + draggedObjectParentColumn);

            }



            //Debug.Log("My parent is "+ this.transform.parent);
            //Debug.Log(" ");



            if (this.transform.parent.gameObject == GameObject.Find("Hand"))
            {                         // Elenxos gia ton parent tis cartas pou ksekina na metakinineite


                for (int i = 1; i <= 7; i++)
                {                                                                                   // Vres tis tade theseis sto tablo kai allakse to to xroma

                    listOfObjectsToHilight.Add(GameObject.Find("Tabletop 1" + i));
                    GameObject.Find("Tabletop 1" + i).transform.GetComponent<Image>().color = new Color32(153, 255, 153, 150);

                }
            }
            else
            {

                Debug.Log("Pesame se else!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ");
                Debug.Log("To draggedObjectParentRow einai " + draggedObjectParentRow);
                Debug.Log("To draggedObjectParentColumn einai " + draggedObjectParentColumn);


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




                int i = 1;
                foreach (var theGameObject in listOfObjectsToHilight)
                {
                    Debug.Log("!!!!! Epanali psi listas: " + i);
                    i++;

                    theGameObject.transform.GetComponent<Image>().color = new Color32(153, 255, 153, 150);                    // Vres tis analoges theseis sto tablo kai svise to xroma to to xroma

                }




            }





            Debug.Log("OnBeginDrag");

            placeholder = new GameObject();
            placeholder.transform.SetParent(this.transform.parent);
            LayoutElement le = placeholder.AddComponent<LayoutElement>();
            le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
            le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
            le.flexibleHeight = 0;
            le.flexibleWidth = 0;

            placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());


            parentToReturnTo = this.transform.parent;
            placeholderParent = parentToReturnTo;
            this.transform.SetParent(this.transform.parent.parent);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        
    }
		
	public void OnDrag (PointerEventData eventData){
        //Debug.Log("OnDrag");

        

        Vector3 correctMousePos = eventData.position;

        //    float correctMousePosX = eventData.position.x;
        //    float correctMousePosY = eventData.position.y;
        //    Vector2 v2 = new Vector2(correctMousePosX, correctMousePosY);
        // correctMousePos.x = correctMousePos.x + 30;
        //correctMousePos.y = correctMousePos.x - 30;

      //  if (isLocalPlayer == true)
     //  {
            this.transform.position = correctMousePos;
      //  }
        

       

        Debug.Log("Mouse position "+ correctMousePos+ "Card Position "+ this.transform.position);

          if (placeholder.transform.parent != placeholderParent)
              {
                  placeholder.transform.SetParent(placeholderParent);
              }

              int newSiblingIndex = placeholderParent.childCount;

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

       


            Debug.Log("OnEndDrag");

            this.transform.SetParent(parentToReturnTo);
            GameObject.Find("PlayerObject(Clone)").GetComponent<PlayerConnection>().myconnection(this.transform , parentToReturnTo);


            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
            GetComponent<CanvasGroup>().blocksRaycasts = true;

            Destroy(placeholder);





            foreach (var theGameObject in listOfObjectsToHilight)
            {
                theGameObject.transform.GetComponent<Image>().color = defaultColorOfCardGrid;                        // Vres tis analoges theseis sto tablo kai svise to xroma to to xroma

            }
            listOfObjectsToHilight.Clear();

            draggedObjectParentRow = 0;
            draggedObjectParentColumn = 0;

            dropPosition = this.transform.position; ///////////////////// den xreisimopoieitai !!
    }
    
}
