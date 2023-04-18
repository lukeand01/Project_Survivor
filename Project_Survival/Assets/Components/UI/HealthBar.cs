using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] Image infectionBar;

    //the whole thing should shake when it takes damage.

    //animation.


    public void UpdateHealth(float current, float total)
    {
        healthBar.fillAmount = current / total;
    }

    public void UpdateInfection(float current, float total)
    {
        infectionBar.fillAmount = current / total;
    }



}
