using JetBrains.Annotations;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, ISaveable
{
    PlayerHandler handler;
    public bool isUsingGun;
    public bool hasGun;
    //take some time for changing stuff.
    public int currentMelee;
    ItemResource meleeReference;
    bool meleeCooldown;

    [Separator("Melee")]
    [SerializeField] MeleeWeapon melee;

    [Separator("Gun")]
    [SerializeField] GameObject gunHolder;
    [SerializeField] Transform gunPoint;
    [SerializeField] Bullet bulletTemplate;
    PlayerGUI gui;
    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
        melee.EventHitSomething += MeleeHitSomething;

    }
    private void Start()
    {
        EquipMelee();
        gui = UIHolder.instance.gui;
        gui.UpdateReceiveGun(hasGun);
        gui.UpdateGun(currentAmmo);
    }

    private void Update()
    {
        if (!isUsingGun) return;

        HandleGun();
        
    }

   

    bool IsRightSideOfScreen()
    {
        return  Input.mousePosition.x > Screen.width / 2.0f;
    }

    #region MELEE
    public void EquipMelee()
    {
        //we look here whne we add an item and when we have spent melee.
        //you only consume itens when the thing is done.


        if (currentMelee <= 0)
        {

            ItemResource resource = handler.inventory.GetResource(ResourceType.Melee);
        
            if(resource != null)
            {
                Debug.Log("found a melee");
                currentMelee = resource.resourceValue;
                meleeReference = resource;
            }
                    
        }

        UIHolder.instance.gui.UpdateMelee(currentMelee);
    }

    public void UseMelee()
    {
        //we play the animation. if it hits something it loses one. if its 

        //we create a little bal in front
        if (meleeCooldown) return;
        if (currentMelee <= 0)
        {
            return;
        }

        melee.gameObject.SetActive(true);
        melee.SetUp(0, 0);

        meleeCooldown = true;
        Invoke(nameof(RefreshMeleeCooldown), 0.5f);
    }

    void RefreshMeleeCooldown() => meleeCooldown = false;

   void MeleeHitSomething()
    {
        currentMelee -= 1;

        if(currentMelee == 0)
        {
            //then we break it
            handler.inventory.SpendItem(meleeReference.itemName);
        }
        EquipMelee();
    }

    #endregion

    #region GUN

    int currentAmmo;

    void HandleGun()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(gunHolder.transform.position);
        var angle = MathF.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gunHolder.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //also when the gun 
        if (IsRightSideOfScreen())
        {
            handler.move.Rotate(1);
            gunHolder.transform.Rotate(new Vector3(0, 0, gunHolder.transform.rotation.z));
        }
        else
        {
            handler.move.Rotate(-1);
            gunHolder.transform.Rotate(new Vector3(180, 0, gunHolder.transform.rotation.z));
        }

        if (Input.GetMouseButtonDown(1) && currentAmmo > 0) UseGun(angle);
    }
    public void StartGun(bool choice)
    {
        if (!hasGun) return;
        isUsingGun = choice;
        gunHolder.SetActive(choice);
        //while it is yes we show the gun. the player speed is lowred, its jumpforce is lowered, and the player looks at the mouse.

        if (choice == true)
        {
            handler.move.isGunJumpDebuff = 0.5f;
            handler.move.isGunMoveDebuff = 2;
        }
        else
        {
            handler.move.isGunJumpDebuff = 0;
            handler.move.isGunMoveDebuff = 0;
        }


    }
    void UseGun(float angle)
    {
        //shot where the mouse is.
        //it goes forward.

        currentAmmo -= 1;
        UIHolder.instance.gui.UpdateGun(currentAmmo);

        Bullet bullet = Instantiate(bulletTemplate, gunPoint.position, Quaternion.identity);
        bullet.gameObject.SetActive(true);

        Vector3 shootDir = (gunPoint.transform.position - transform.position).normalized;

        bullet.SetUpBullet(10, shootDir, gameObject);

    }

    public void ReceiveGun(ItemResource gunItem)
    {
        Debug.Log("received a gun");
        if (hasGun)
        {
            //we turn the pistol into ammo.
            currentAmmo += 3;
            UIHolder.instance.gui.UpdateGun(currentAmmo);
        }
        else
        {
            hasGun = true;
            UIHolder.instance.gui.UpdateReceiveGun(hasGun);
            UIHolder.instance.gui.UpdateGun(currentAmmo);
        }
        

    }
    public void UpdateAmmo(int ammo)
    {
        Debug.Log("should update ammo");
        currentAmmo = ammo;
        UIHolder.instance.gui.UpdateGun(ammo);
    }




    #endregion

    #region SAVE
    public object CaptureState()
    {
        return new SaveData
        {
            hasGun = hasGun,
            currentMelee = currentMelee
        };
    }

    public void RestoreState(object state)
    {

        var save = (SaveData)state;

        hasGun = save.hasGun;
        currentMelee = save.currentMelee;

        Invoke(nameof(GunSaveOrder), 0.01f);
        
    }

    void GunSaveOrder()
    {
        gui.UpdateReceiveGun(hasGun);

        if (hasGun)
        {
            //we update its ammo
            int ammo = handler.inventory.GetAmmo();
            gui.UpdateGun(ammo);
        }

        gui.UpdateMelee(currentMelee);
    }


    [Serializable]
    struct SaveData
    {
        public bool hasGun;
        public int currentMelee;
    }
    #endregion
}
