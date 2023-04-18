using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : TriggerBase
{
    [SerializeField] UnityEvent unityEvent;
    public override void Act()
    {
        unityEvent.Invoke();
    }

}
