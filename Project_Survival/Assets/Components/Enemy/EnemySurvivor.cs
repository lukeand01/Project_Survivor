using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySurvivor : EnemyBase
{

    [SerializeField] bool isBandit = false;
    [SerializeField] bool isArmed = false;
    [SerializeField] GameObject gunHolder;
    [SerializeField] MeleeWeapon meleeHolder;
    float weaponRange = 0;

    private void Awake()
    {
        if (isArmed)
        {
            gunHolder.SetActive(true);
            meleeHolder.gameObject.SetActive(false);
            weaponRange = 5;
        }
        else
        {
            gunHolder.SetActive(false);
            meleeHolder.gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        if (isBandit)
        {
            BanditBehavior();
        }
        else
        {
            SurvivorBehavior();
        }
    }


    #region BASE
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

    #endregion

    #region BANDIT
    void BanditBehavior()
    {
        if (!IsTick()) return;

        Transform target = GetTarget();

        if (target == null) return;

        Debug.Log("found my target " + target.gameObject.name);

        if (isArmed)
        {
            //move the gun towards the target.
            var dir = target.position - Camera.main.WorldToScreenPoint(gunHolder.transform.position);
            var angle = MathF.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            gunHolder.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (isAttackCooldown) return;

        
       if(IsTargetInAttackRange(target, weaponRange))
        {
            Attack();
            return;
        }

        if (IsTargetInDetectionRange(target, weaponRange))
        {
            Move(GetTargetDir(target));   
        }
    }

    void Attack()
    {
        if (isArmed)
        {

        }
        else
        {

        }
    }

    #endregion

    #region SURVIVOR
    void SurvivorBehavior()
    {

    }

    #endregion






}
