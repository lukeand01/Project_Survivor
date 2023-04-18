using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public void Interact(bool isInteracting = true);
    public void InteractUI(bool choice);
    public bool IsInteractable();


}
