using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IInteractable, ISaveable
{
    [SerializeField] InventorySave inventorySave;
    [SerializeField] List<ItemClass> itemList = new List<ItemClass>();
    [SerializeField] InteractUI interactUI;

    float totalInteract = 0.5f;
    float currentInteract;
    bool open;

    [SerializeField] GameObject closeLid;
    [SerializeField] GameObject openLid;

    public void Interact(bool isInteracting)
    {
        //we charge it a bit and then open instantly getting all loot.
        //how to know when you are no logner interacting?
        if (isInteracting)
        {
            currentInteract += Time.deltaTime;

            if(currentInteract > totalInteract)
            {
                OpenBox();
                currentInteract = 0;
                open = true;
                interactUI.gameObject.SetActive(false);
            }
        }
        else
        {
            currentInteract = 0;
        }

        interactUI.Charge(currentInteract, totalInteract);

    }

    void OpenBox()
    {
        openLid.SetActive(true);
        closeLid.SetActive(false);
        for (int i = 0; i < itemList.Count; i++)
        {
            PlayerHandler.instance.ReceiveItem(itemList[i]);
        }
        itemList.Clear();
    }

    public void InteractUI(bool choice)
    {
        interactUI.gameObject.SetActive(choice);
    }

    public bool IsInteractable()
    {
        if (open) return false;
        return true;
    }

    #region SAVE
    public object CaptureState()
    {
        //i dont need a savedata struct just the saveinventory
        if (inventorySave == null)
        {
            Debug.Log("something wrong");
            return null;
        }
        Debug.Log("capture the state of the box");
        inventorySave.SaveInventory(itemList);

        return null;
    }

    public void RestoreState(object state)
    {
        if (inventorySave == null)
        {
            Debug.Log("something wrong");
            return;
        }


        itemList = inventorySave.GetInventory();


        if(itemList.Count <= 0)
        {
            open = true;
            openLid.SetActive(true);
            closeLid.SetActive(false);
        }
        else
        {
            open = false;
            openLid.SetActive(false);
            closeLid.SetActive(true);
        }

    }

    #endregion
}
