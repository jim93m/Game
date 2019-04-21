using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;




public class mouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static GameObject mouseOverObject=null;
    

    void Start() {
      

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOverObject = this.gameObject;
        if (this.transform.parent.gameObject == GameObject.Find("Hand")) {

          


        }


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOverObject = null;

        if (this.transform.parent.gameObject == GameObject.Find("Hand"))
        {

        

        }

    }


}
