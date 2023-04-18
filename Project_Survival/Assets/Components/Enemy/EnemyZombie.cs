using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZombie : EnemyBase
{
    //it is simple. it goes after player if its in teh same level. thats it.
    //it also attacks players.


    private void Update()
    {
        if (DEBUG_CANNOTMOVE) return;

        //if there is a wall ahead we stop.

        

        if (ShouldFall()) return;

        if (isAttackCooldown)
        {
            Move(0);
            return;
        }


        if (!IsTick()) return;

        Transform target = GetHuman();

        //however
        if (target == null)
        {
            //walk around.
            Patrol();
        }
        else
        {

            if (IsWallAhead() && GetTargetDir(target) == currentDir)
            {                              
                Move(0);
                return;
            }


            if (IsTargetInAttackRange(target))
            {
                //attack
               
                return;
            }

            Move(GetTargetDir(target));
        }
        
    }

    void RefreshAttack() => isAttackCooldown = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) return;
        if (collision.gameObject.tag != "Human") return;
        if (isAttackCooldown) return;

  
        //otherwise we deal damage to it.
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;


        int infectionRoll = Random.Range(0, 100);
        int infectionStrenght = data.infectionClass.GetStrenght(infectionRoll);
        int bleedingRoll = Random.Range(0, 100);
        int bleedingStrenght = data.bleedingClass.GetStrenght(bleedingRoll);


        damageable.TakeDamage(data.enemyDamage, transform, 1, bleedingStrenght, infectionStrenght);
        isAttackCooldown = true;
        Invoke(nameof(AttackCooldownOrder), 0.5f);
    }


    void AttackCooldownOrder() => isAttackCooldown = false;

    //but if its hit the player.

    //zombie deal damage on touch and players need to attack.

}
