using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonEvent : ButtonBase
{
    public UnityEvent buttonEvent;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("point click");
        buttonEvent.Invoke();
    }

    
}
