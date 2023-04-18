using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventorySave : ScriptableObject
{
    //this is where we are going to hold all the info about the inventory.
    public List<ItemClass> itemList = new List<ItemClass>();

    public void SaveInventory(List<ItemClass> itemList)
    {
        this.itemList.Clear();
        for (int i = 0; i < itemList.Count; i++)
        {

            ItemClass item = new ItemClass(itemList[i].data, itemList[i].quantity);
            this.itemList.Add(item);

        }
    }
    public List<ItemClass> GetInventory()
    {
        List<ItemClass> newItemList = new List<ItemClass>();

        for (int i = 0; i < itemList.Count; i++)
        {
            ItemClass newItem = new ItemClass(itemList[i].data, itemList[i].quantity);
            newItemList.Add(newItem);
        }


        return newItemList;
    }

    public void ClearInventory() => itemList.Clear();

}
