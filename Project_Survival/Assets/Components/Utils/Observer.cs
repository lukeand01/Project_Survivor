using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public static Observer instance;

    private void Awake()
    {
        instance = this;
    }

    //public event Action<int> EventCraftPotion;
    //public void OnCraftPotion(int empty) => EventCraftPotion?.Invoke(empty);

    public event Action<string> EventGlobalDescriptor;
    public void OnGlobalDescriptor( string main = "") => EventGlobalDescriptor?.Invoke(main);

    public event Action<string> EventNarration;
    public void OnNarration(string main = "") => EventNarration?.Invoke(main);


}

//does it have rounds? yes, but you have to do something to trigger the next round. which grants you better buffs, components.
//time itself makes 