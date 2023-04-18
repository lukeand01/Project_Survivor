using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSave : TriggerBase
{

    public override void Act()
    {
        Debug.Log("saved " + gameObject.name);
        SaveHandler.instance.Save("First");
    }


}
