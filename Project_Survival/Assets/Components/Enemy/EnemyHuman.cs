using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHuman : EnemyBase
{
    //we hav ebandits, survivors
    //but what if we want to fight with the survivors?

    

    [Separator("HUMAN")]
    public bool isBandit;
    public bool isArmed;

    [SerializeField] GameObject gunHolder;
    [SerializeField] MeleeWeapon meleeHolder;


    private void Awake()
    {
        if (isArmed)
        {
            gunHolder.SetActive(true);
            meleeHolder.gameObject.SetActive(false);
        }
        else
        {
            gunHolder.SetActive(false);
            meleeHolder.gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        if (isBandit) BanditBehavior();
        else SurvivorBehavior();

    }
   
    void BanditBehavior()
    {
        //bandit attacks at first sight.
        //they chase even after going over ledges or over walls.
        

        //attacks the closest enemy. either zombie or playeer.

        if (!IsTick()) return;

        Transform target = GetTarget();

        if (target == null) return;

        //we attack teh target.
        //aim at the target.
        if (isArmed)
        {
            //the gun will be always aiming the target fella.
            var dir = target.position - Camera.main.WorldToScreenPoint(gunHolder.transform.position);
            var angle = MathF.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            gunHolder.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


        if (isAttackCooldown) return;


        float targetDistance = Vector3.Distance(transform.position, target.position);

        if (targetDistance < data.enemyAttackRange && !isArmed)
        {
            Debug.Log("attack");
            Attack();
            return;
        }

        if (targetDistance < data.enemyDetectRange)
        {
            //move to target.
            Debug.Log("moving");

            if (isArmed)
            {
                //then we attack
                Debug.Log("bitch is armed");
                Shoot();
                
            }


            Move(GetTargetDir(target));
            return;
        }
        


    }

    void Attack()
    {
        isAttackCooldown = true;
        //play anim and deal damage.            
        meleeHolder.SetUp(10, 50);

    }


    void Shoot()
    {
        isAttackCooldown = true;
        
        //shoot
    }

    void RefreshAttackCooldown() => isAttackCooldown = false;

    Transform GetTarget()
    {
        //the target needs to have idamageable.
        //the target needs to be in the his y axis.
        //the target is thee closest to him.

        RaycastHit2D[] rayList = Physics2D.CircleCastAll(transform.position, data.enemyDetectRange, Vector2.zero, 10);
        if (rayList.Length <= 0) return null;

        float closestTarget = 0;
        Transform currentTarget = null;

        for (int i = 0; i < rayList.Length; i++)
        {
            if (DifferenceInYTooHigh(rayList[i].transform, 5))
            {
                continue;
            }

            IDamageable damageble = rayList[i].collider.gameObject.GetComponent<IDamageable>();

            if (damageble == null) continue;
            if (damageble.IsDead()) continue;

            float distance = Vector3.Distance(rayList[i].transform.position, transform.position);
            if (currentTarget == null)
            {
                closestTarget = distance;
                currentTarget = rayList[i].transform;
            }
            else
            {
                if (closestTarget > distance)
                {
                    closestTarget = distance;
                    currentTarget = rayList[i].transform;
                }
            }
        }

        return currentTarget;
    }


    void SurvivorBehavior()
    {
        //will warn the player. if the player gets close or attacks them in anyway.
        //jumping over ledges or jumping over a wall will cease the 
        //they prioritize the closest target.


        if (isAttackCooldown) return;

        Debug.Log("survivor behavior");

        if (IsTargetInAttackRange(PlayerHandler.instance.transform))
        {
            CancelWarn();
            //play bat animation.

            return;
        }

        if (IsTargetInDetectionRange(PlayerHandler.instance.transform))
        {
            CancelWarn();
            //go after the player. if the player jumps somewhere.
            Move(GetPlayerDir());

            Debug.Log("chase");
            return;
        }


        if (IsWarnPlayer())
        {
            if (!PlayerHandler.instance.IsWarned)
            {
                Debug.Log("warn player");
                //then we narrate it.
                Observer.instance.OnNarration(GetWarning());
                PlayerHandler.instance.IsWarned = true;
            }


            
        }
        
        //but if you are agrod it takes a bit longer to flee.


    }
    
    void CancelWarn()
    {
        if (PlayerHandler.instance.IsWarned)
        {
            Observer.instance.OnNarration();
        }

        PlayerHandler.instance.IsWarned = false;
    }

    string GetWarning()
    {
        int random = UnityEngine.Random.Range(0, 5);

        if(random == 0)
        {
            return "Please dont get any closer";
        }
        if(random == 1)
        {
            return "We are not afraid of killing! fuck off!";
        }
        if(random == 2)
        {
            return "Go get killed by someone else!";
        }
        if(random == 3)
        {
            return "we have weapons and we are not afraid to use it!";
        }
        if(random == 4)
        {
            return "Dont get close!";
        }
        if(random == 5)
        {
            return "I will survive this. do not teste me!";
        }

        return "";

    }

    bool IsWarnPlayer()
    {
        //the distance they will notice. and the distance they will stop chasing.
        //but alos its in the same x.

        float differenceY = Mathf.Abs(PlayerHandler.instance.transform.position.y - transform.position.y);

        if (differenceY > 5) return false;

        float distance = Vector3.Distance(PlayerHandler.instance.transform.position, transform.position);
        return data.enemyWarnRange > distance;

    }

    bool ForgetPlayer()
    {
        float distance = Vector3.Distance(PlayerHandler.instance.transform.position, transform.position);
        return data.enemyWarnRange > distance;
    }


    IEnumerator AttackProcess()
    {
        isAttackCooldown = true;

        yield return new WaitForSeconds(data.enemyAttackSpeed);
        isAttackCooldown = false;
    }
}
