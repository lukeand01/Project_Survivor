using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    //now how i will do the stuff from each item.
    public string itemName;
    [TextArea]public string itemDescription;
    public Sprite itemPortrait;

    public virtual void Act()
    {



    }

    public virtual ItemResource GetResource() => null;

}
[System.Serializable]
public class ItemClass
{
    public ItemData data;
    public int quantity;
    public ItemClass(ItemData data, int quantity = 1)
    {
        this.data = data;
        this.quantity = quantity;
    }

    public void RemoveQuantity(int quantity = 1)
    {
        this.quantity -= quantity;
    }
    public void IncreaseQuantity(int quantity = 1)
    {
        this.quantity += quantity;
    }
    public bool ShouldExist()
    {
        return quantity > 0;
    }
}