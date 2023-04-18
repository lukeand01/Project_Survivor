using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    public void TakeDamage(float damage, Transform attacker, float pushModifier = 1, int bleedingStrenght = 0, int infectionStrenght = 0);

    public bool IsDead();
}
