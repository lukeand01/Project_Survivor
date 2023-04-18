using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorTeleport : MonoBehaviour, IInteractable
{
    //it telports you to another door.
    //may need lockpick to be opened.

    [SerializeField] bool needLockPick;
    bool open;
    [SerializeField] InteractUI interactUI;
    [SerializeField] DoorTeleport targetDoor;

    float totalInteract = 0.7f;
    float currentInteract;

    float totalInteractLockpick = 1.5f;
    float currentInteractLockpick;

    public void Interact(bool isInteracting = true)
    {

        if (!isInteracting)
        {
            //then the charge is zero for both.
            currentInteractLockpick = 0;
            currentInteract = 0;
            interactUI.Charge(0, 1);
            return;
        }

        if(needLockPick && !open)
        {
            if (!PlayerHandler.instance.inventory.GetResource(ResourceType.Lockpick)) return;

            if(currentInteractLockpick > totalInteractLockpick)
            {
                open = true;
                PlayerHandler.instance.inventory.SpendItem("Lockpick");
                OpenDoor();
                interactUI.ControlLockpick(false, false);
            }
            else
            {
                currentInteractLockpick += Time.deltaTime;
            }

            
            interactUI.Charge(currentInteractLockpick, totalInteractLockpick);
                
            return;
        }

        if(currentInteract > totalInteract)
        {
            open = true;
            OpenDoor();
        }
        else
        {
            currentInteract += Time.deltaTime;
        }
        interactUI.Charge(currentInteract, totalInteract);
        //open normally
    }




    
    void OpenDoor()
    {
        Debug.Log("open door");
        UIHolder.instance.FadeTransition(targetDoor.transform);

    }



    public void InteractUI(bool choice)
    {
        interactUI.gameObject.SetActive(choice);

        if(needLockPick && !open)
        {
            ItemResource resource = PlayerHandler.instance.inventory.GetResource(ResourceType.Lockpick);

            if(resource == null)
            {
                interactUI.ControlLockpick(choice, false);
            }
            else
            {
                interactUI.ControlLockpick(choice, true);
            }
            

        }

    }

    public bool IsInteractable()
    {
        return true;
    }
}
