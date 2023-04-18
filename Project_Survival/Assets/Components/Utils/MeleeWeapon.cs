using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    //this detects collision.
    float damage;
    int bleedingChance;
    Animator anim;


    public event Action EventHitSomething;
    public void OnHitSomething() => EventHitSomething?.Invoke();

    public void SetUp(float damage, int bleedingChance)
    {
        this.damage = damage;
        this.bleedingChance = bleedingChance;

        //set up makes the animation trigger.
        if(anim == null) anim = GetComponent<Animator>();
        anim.Play("MeleeWeapon_Attack");
        Invoke(nameof(CloseMelee), 0.52f);
    }

    //it can hit as many fellas as possible. but not the same ones. if thats happening need a fix.

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //both weapons can interact with zombies. it just can never interact with itself.
        
        if(collision.gameObject.layer == 7 || collision.gameObject.layer == 9)
        {
            if(transform.parent.gameObject == collision.gameObject)
            {
                return;
            }

            //otherwise we deal damage to the thing.
            IDamageable damageable = collision.GetComponent<IDamageable>();

            if (damageable == null) return;
            OnHitSomething();
            damageable.TakeDamage(damage, transform, 1, bleedingChance);

        }


    }


    void CloseMelee()
    {
        gameObject.SetActive(false);
    }

}



