using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class ButtonBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerMoveHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
       
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerMove(PointerEventData eventData)
    {
        
    }
}
