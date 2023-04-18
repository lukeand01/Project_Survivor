using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Armor")]
public class ItemArmor : ItemData
{

    public override void Act()
    {
        //adds armor.
        //there is a limit to armor. and armor makes the characters slower.
        PlayerHandler.instance.resource.AddArmor();

    }
}
