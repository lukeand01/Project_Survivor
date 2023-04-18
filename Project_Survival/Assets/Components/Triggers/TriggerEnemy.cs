using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemy : TriggerBase
{
    [SerializeField]EnemyBase[] enemyList;

    public override void Act()
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyList[i].DEBUG_CANNOTMOVE = false;
        }
    }

}
