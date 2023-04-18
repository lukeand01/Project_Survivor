using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalDescriptorUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptorText;
    GameObject holder;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        Observer.instance.EventGlobalDescriptor += ReceiveOrder;
    }

    private void Update()
    {
        if (holder.activeInHierarchy)
        {
            holder.transform.position = Input.mousePosition + new Vector3(100, 0, 0);
        }
    }


    void ReceiveOrder(string description)
    {
        if(description == "")
        {
            //then we turn it off.
            holder.SetActive(false);
        }
        else
        {
            holder.SetActive(true);
            descriptorText.text = description;
        }
        
    }

}
