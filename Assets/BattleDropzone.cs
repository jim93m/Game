using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleDropzone : MonoBehaviour,IDropHandler,IPointerEnterHandler, IPointerExitHandler{

    public int idX;
    public int idY;
    public void OnPointerEnter(PointerEventData eventData)
    {

        if (eventData.pointerDrag == null) { return; }

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && this.transform.childCount<1)                                   // childCount<1 den epitrepei na bei placeholder mesa se dropzone pou uparzei idi mia karta
        {
            Debug.Log("The parent of the druged card is " + d.transform.parent);
            d.placeholderParent = this.transform;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) { return; }

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeholderParent == this.transform)
        {
            d.placeholderParent = d.parentToReturnTo;
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was droped on " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

        if (d != null && d.placeholderParent.childCount<2 && d.validMoveList.Contains(this.gameObject) )    // Edw ginetai elenxos an to dropzone pou pas na kaneis drop tin karta anikei sta valid dropzones
        {
            d.parentToReturnTo = this.transform;
        }

    }



}
