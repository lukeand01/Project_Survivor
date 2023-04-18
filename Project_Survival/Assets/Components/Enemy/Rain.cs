using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    //gravity does the whole job. this just detects if it has touched the player

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Zombie")
        {
            IDamageable damage = collision.GetComponent<IDamageable>();

            if (damage == null) return;

            damage.TakeDamage(10, transform);
            Destroy(gameObject);
        }
    }
}
