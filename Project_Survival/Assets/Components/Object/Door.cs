using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, ISaveable
{
    [SerializeField] bool needSledgehammer;
    [SerializeField] bool isInteractble = true;
    [ConditionalField(nameof(needSledgehammer))] GameObject needSledgehammerImage;
    [SerializeField] InteractUI interactUI;
    GameObject body;
    [SerializeField] bool isOpen;

    float totalInteract = 1.5f;
    float currentInteract;

    private void Awake()
    {
        body = transform.GetChild(0).gameObject;

        if (isOpen)
        {
            body.gameObject.SetActive(false);
        }
        else
        {
            body.gameObject.SetActive(true);
        }

    }

    void OpenDoor()
    {
        isOpen = true;

        body.gameObject.SetActive(false);
    }

    public void CloseDoor()
    {
        isOpen = false;

        body.gameObject.SetActive(true);
    }


    public void Interact(bool isInteracting = true)
    {
        if (!IsInteractable())
        {
            return;
        }

        if (needSledgehammer && !PlayerHandler.instance.hasSledgehammer)
        {
            return;
        }


        if (isInteracting)
        {
            currentInteract += Time.deltaTime;

            if (currentInteract > totalInteract)
            {
                OpenDoor();
                currentInteract = 0;
                isOpen = true;
                interactUI.gameObject.SetActive(false);
            }
        }
        else
        {
            currentInteract = 0;
        }

        interactUI.Charge(currentInteract, totalInteract);
    }



    //charge 


    public void InteractUI(bool choice)
    {
        if (!IsInteractable())
        {
            interactUI.gameObject.SetActive(false);
            return;
        }
        interactUI.gameObject.SetActive(choice);


        if (needSledgehammer && !PlayerHandler.instance.hasSledgehammer)
        {
            interactUI.ControlCannot(true);
            return;
        }
        interactUI.ControlCannot(false);

    }

    public bool IsInteractable()
    {
        if (!isInteractble) return false;
        if (isOpen) return false;    
        return true;
    }

    #region SAVE
    public object CaptureState()
    {
        return new SaveData
        {
            isOpen = isOpen
        };
    }

    public void RestoreState(object state)
    {
        var save = (SaveData)state;

        isOpen = save.isOpen;

        if (!isOpen)
        {
            Destroy(body);
        }

    }

    [Serializable]
    struct SaveData
    {
        public bool isOpen;
    }
    #endregion
}
