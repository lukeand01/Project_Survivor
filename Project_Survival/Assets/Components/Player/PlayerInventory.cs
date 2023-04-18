using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, ISaveable
{
    public InventorySave inventorySave;
    public List<ItemClass> inventoryList = new List<ItemClass>();
    PlayerHandler handler;

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        

    }
    private void Start()
    {
        UpdateInventoryUI();
    }

    public void CheckThisSide(int index)
    {
        if (!inventoryList[index].ShouldExist())
        {
            inventoryList.RemoveAt(index);
        }
    }
    public void UpdateInventoryUI()
    {
        UIHolder.instance.inventory.UpdateUI(inventoryList);
    }

    public void ReceiveItem(ItemClass newItem)
    {
        //we just check if there is one slot of it.

        int index = GetItemIndex(newItem);

       

        if(index == -1)
        {
            //create new one
            inventoryList.Add(newItem);
        }
        else
        {
            //update.
            inventoryList[index].IncreaseQuantity(newItem.quantity);
        }

        UIHolder.instance.inventory.AddNotification(newItem);
        UpdateInventoryUI();
        if (newItem.data.GetResource() != null)
        {
            if(newItem.data.GetResource().resourceType == ResourceType.Melee) handler.combat.EquipMelee();
            if(newItem.data.GetResource().resourceType == ResourceType.Ammo) handler.combat.UpdateAmmo(GetAmmo());
            if(newItem.data.GetResource().resourceType == ResourceType.Gun) handler.combat.ReceiveGun(newItem.data.GetResource());
            if(newItem.data.GetResource().resourceType == ResourceType.Sledgehammer) handler.hasSledgehammer = true;

        }
    }

    public int GetAmmo()
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {
            ItemResource resource = inventoryList[i].data.GetResource();
            if (resource != null)
            {
                if (resource.resourceType == ResourceType.Ammo) return inventoryList[i].quantity;
            }
        }
        return 0;


    }

    public void SpendItem(string id)
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].data.itemName == id)
            {
                if (inventoryList[i].data.GetResource() != null)
                {
                    if (inventoryList[i].data.GetResource().resourceType == ResourceType.Ammo) handler.combat.UpdateAmmo(GetAmmo());
                }
            }
        }
        UpdateInventoryUI();
        
    }

    int GetItemIndex(ItemClass newItem)
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (newItem.data == inventoryList[i].data) return i;
        }

        return -1;
    }

   public ItemResource GetResource(ResourceType resourceType)
    {

        for (int i = 0; i < inventoryList.Count; i++)
        {

            ItemResource resource = inventoryList[i].data.GetResource();
            if (resource != null)
            {
                if (resource.resourceType == resourceType) return resource;
            }
        }

        return null;
    }

    #region SAVE
    public object CaptureState()
    {
        //i dont need a savedata struct just the saveinventory
        if(inventorySave == null)
        {
            Debug.Log("something wrong");
            return null;
        }


        inventorySave.SaveInventory(inventoryList);

        return null;
    }

    public void RestoreState(object state)
    {
        if(inventorySave == null)
        {
            Debug.Log("something wrong");
            return;
        }

        inventoryList = inventorySave.GetInventory();
        UIHolder.instance.inventory.UpdateUI(inventoryList);
    }

    
    #endregion
}

