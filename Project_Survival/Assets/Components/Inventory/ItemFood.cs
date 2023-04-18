using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Food")]
public class ItemFood : ItemData
{
    public override void Act()
    {
        //heal the player.
        PlayerHandler.instance.resource.HealHealth(20);
    }
}
