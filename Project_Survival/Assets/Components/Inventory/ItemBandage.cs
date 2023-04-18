using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Bandage")]
public class ItemBandage : ItemData
{
    public override void Act()
    {
        //stop bleeding and heal the player a bit.
        PlayerHandler.instance.resource.CureBleeding();
    }
}
