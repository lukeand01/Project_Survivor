using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalHandler : MonoBehaviour
{
    public static LocalHandler instance;
    [SerializeField] Transform initialPos;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void StartLocal()
    {
        PlayerHandler.instance.transform.position = initialPos.position;
    }
    
    public void EndDemo()
    {

    }

}
