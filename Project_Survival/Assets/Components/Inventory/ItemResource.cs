using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Resource")]
public class ItemResource : ItemData
{
    //spendable resources that are not usable through interaction
    public int resourceValue;
    public ResourceType resourceType;

    public override ItemResource GetResource()
    {
        return this;
    }

}

public enum ResourceType
{
    Ammo,
    Lockpick,
    Melee,
    Gun,
    Sledgehammer
}