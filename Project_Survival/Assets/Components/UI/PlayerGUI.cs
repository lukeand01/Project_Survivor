using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    [SerializeField] HealthBar bar;

    [SerializeField] StatusUnit infectionUnit;
    [SerializeField] StatusUnit bleedingUnit;

    [Separator("ARMOR")]
    [SerializeField] GameObject armorHolder;
    [SerializeField] TextMeshProUGUI armorText;

    [Separator("MELEE")]
    [SerializeField] GameObject meleeHolder;
    [SerializeField] TextMeshProUGUI meleeAmmo;
    [SerializeField] TextMeshProUGUI meleeText;
    [SerializeField] TextMeshProUGUI noMeleeText;

    [Separator("GUN")]
    [SerializeField] GameObject gunHolder;
    [SerializeField] TextMeshProUGUI gunAmmoText;
    [SerializeField] TextMeshProUGUI noGunAmmoText;

    [Separator("COOLDOWNS")]
    [SerializeField] StatusUnit dashUnit;

    public void UpdateHealth(float current, float total) => bar.UpdateHealth(current, total);
   
    public void UpdateInfection(float current, float total) => bar.UpdateInfection(current, total);


    public void UpdateStatusInfection(StatusClass info)
    {
        if(!info.exist)
        {
            infectionUnit.gameObject.SetActive(false);
        }
        else
        {
            infectionUnit.gameObject.SetActive(true);
            infectionUnit.UpdateUI(info);
        }

    }
    public void UpdateStatusBleeding(StatusClass info)
    {
        if (!info.exist)
        {
            bleedingUnit.gameObject.SetActive(false);
        }
        else
        {
            bleedingUnit.gameObject.SetActive(true);
            bleedingUnit.UpdateUI(info);
        }
    }

    public void UpdateArmor(float armor)
    {
        if(armor == 0)
        {
            armorHolder.SetActive(false);
        }
        else
        {
            armorHolder.SetActive(true);
            armorText.text = armor.ToString();
        }
    }
    public void BreakArmor()
    {
        //its nothing but a breaking sound for now.
        Debug.Log("broke");
    }


    public void UpdateMelee(int currentMelee)
    {
        if (currentMelee <= 0)
        {
           noMeleeText.gameObject.SetActive(true);
           meleeAmmo.gameObject.SetActive(false);
           return;
        }

        noMeleeText.gameObject.SetActive(false);
        meleeAmmo.gameObject.SetActive(true);
        meleeAmmo.text = currentMelee.ToString();


    }

    public void UpdateReceiveGun(bool choice)
    {
        gunHolder.SetActive(choice);
    }
    public void UpdateGun(int gunAmmo)
    {
        if(gunAmmo > 0)
        {
            noGunAmmoText.gameObject.SetActive(false);
            gunAmmoText.gameObject.SetActive(true);
            gunAmmoText.text = gunAmmo.ToString();
        }
        else
        {
            noGunAmmoText.gameObject.SetActive(true);
            gunAmmoText.gameObject.SetActive(false);
        }
    }


    void ControlMeleeHolder(bool choice)
    {
        if (choice)
        {

        }
        else
        {

        }
    }


    public void UpdateDashCooldown(float current, float total)
    {
        if (current > 0) dashUnit.gameObject.SetActive(true);
        dashUnit.UpdateOtherUI(current, total);
    }

}
