using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Medicine")]
public class ItemMedicine : ItemData
{

    public override void Act()
    {
        //stoips infection and heals the player a bit.
        PlayerHandler.instance.resource.CureInfection();
    }
}
