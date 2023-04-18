using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarningWindow : MonoBehaviour
{
    GameObject holder;
    [SerializeField] TextMeshProUGUI yesText;
    [SerializeField] TextMeshProUGUI noText;
    [SerializeField] TextMeshProUGUI warnText;


    public event Action EventWarningYes;
    public void OnWarningYes() => EventWarningYes?.Invoke();

    public event Action EventWarningNo;
    public void OnWarningNo() => EventWarningNo?.Invoke();

    public bool isActive;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }
    public void WarningReset()
    {
        //remove this thing.
        holder.gameObject.SetActive(false);
        isActive = false;
    }

    public void StartWarning(string title, string first = "Confirm", string second = "Cancel")
    {
        //we create this to ask certain things. for now its a simple yes or no.
        holder.gameObject.SetActive(true);
        isActive = true;
        warnText.text = title;
        yesText.text = first;
        noText.text = second;
    }

    public void ClearEvents()
    {
        EventWarningNo = delegate { };
        EventWarningYes = delegate { };
    }

    public void Yes()
    {
        WarningReset();
        OnWarningYes();
    }
    public void No()
    {
        WarningReset();
        OnWarningNo();
    }

    //if you ever press the button.




}
