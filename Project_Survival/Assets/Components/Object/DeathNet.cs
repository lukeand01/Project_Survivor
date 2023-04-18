using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathNet : MonoBehaviour
{
    //one shot the fella. and lock the camera.
    [SerializeField] bool actualDeathNet = true;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;

        Debug.Log("collided");

        if (actualDeathNet)
        {
            Invoke(nameof(DealDamage), 0.4f);
            PlayerHandler.instance.cam.LockCamera();
        }
        else
        {
            PlayerHandler.instance.resource.TakeDamage(1000, transform);
        }
        
    }

    void DealDamage()
    {
        PlayerHandler.instance.resource.TakeDamage(1000, transform);
    }

}
