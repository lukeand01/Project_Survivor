using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractUI : MonoBehaviour
{
    //charge the thing and do other effects.

    [SerializeField] Image chargeUI;
    [SerializeField] GameObject cannotBackground;
    [SerializeField] GameObject cannotIcon;
    [SerializeField] TextMeshProUGUI lockpickText;

    public void ControlLockpick(bool choice, bool haslockpick)
    {
        ControlCannot(choice);
        lockpickText.gameObject.SetActive(choice);

        if (haslockpick)
        {
         
            lockpickText.text = "Use lockpick";
            lockpickText.color = Color.white;
        }
        else
        {
            chargeUI.gameObject.SetActive(false);
            lockpickText.text = "Need lockpick";
            lockpickText.color = Color.red;
        }

        
    }

    public void ControlCannot(bool choice)
    {
        cannotBackground.SetActive(choice);
        cannotIcon.SetActive(choice);
    }

    public void Charge(float current, float total)
    {
        chargeUI.fillAmount = current / total;
    }

}
