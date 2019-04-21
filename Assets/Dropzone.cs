using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dropzone : MonoBehaviour,IDropHandler,IPointerEnterHandler, IPointerExitHandler{

   

    public void OnPointerEnter(PointerEventData eventData) {


    }

    public void OnPointerExit(PointerEventData eventData)
    {


    }

    public void OnDrop(PointerEventData eventData)
    {
     //   Debug.Log(eventData.pointerDrag.name + "was droped on " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

      //  d.parentToReturnTo = this.transform;
        
        if(d != null)
        {
           // d.parentToReturnTo = this.transform;                                //An auti einai sxoliasmeni den boroun na epistrepsoun kartes sto xeri, se adithsi me ta kanonika dropzones
        }
        
    }



}
