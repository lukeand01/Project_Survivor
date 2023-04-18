using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float enemyHealth = 5;
    public float enemySpeed = 1;
    public float enemyAttackRange = 1;
    public float enemyDetectRange = 1;
    public float enemyWarnRange = 1;
    public float enemyAttackSpeed = 1;
    public float enemyDamage = 2;

    public bool isZombie;

    [ConditionalField(nameof(isZombie))] public StatusHolderClass infectionClass;

    public StatusHolderClass bleedingClass;
}

[System.Serializable]
public class StatusHolderClass
{

   public List<StatusChanceClass> statusList = new List<StatusChanceClass>();

    [System.Serializable]
   public class StatusChanceClass
    {
        public int strenght;
        [Range(0, 100)] public int chance;
    }

    public int GetStrenght(int roll)
    {
        int strenght = 0;
        for (int i = 0; i < statusList.Count; i++)
        {
            if (roll < statusList[i].chance) strenght = statusList[i].strenght;
        }


        return strenght;
    }
}