using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEditor.Search;
using UnityEngine;

public class PlayerResource : MonoBehaviour, IDamageable, ISaveable
{
    PlayerHandler handler;
    [SerializeField] float maxHealth = 50;
    float currentHealth = 0;
    bool isDead = false;

    public float  currentInfection = 0;
    float currentArmor = 0;
    //bleeding can stack.

    //infections can stack

    [SerializeField]StatusClass currentStatusInfection = null;
    [SerializeField]StatusClass currentStatusBleeding = null;

    //ArmorClass armor;
    PlayerGUI gui;

    private void Awake()
    {
        currentHealth = maxHealth;
        handler = GetComponent<PlayerHandler>();
    }
    private void Start()
    {


        gui = UIHolder.instance.gui;

        gui.UpdateHealth(currentHealth, maxHealth);
        gui.UpdateInfection(currentInfection, maxHealth);
        gui.UpdateArmor(currentArmor);
    }


    void HandleStatus()
    {
        if(currentStatusInfection.exist)
        {
            currentStatusInfection.RunTimer();
            if (currentStatusInfection.CanCall())
            {
                currentStatusInfection.CallInfection();
                //update the ui.
            }

        }
        
        if(currentStatusBleeding.exist)
        {
            currentStatusBleeding.RunTimer();
            if (currentStatusBleeding.CanCall())
            {
                currentStatusBleeding.CallBleeding();
            }
        }
    }

    private void Update()
    {

        HandleStatus();



        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //deal damage.
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //recover health
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //add infection
            Debug.Log("get infected");
            AddInfection();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //cure infection.
            CureInfection();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //add bleeding
            AddBleeding();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            //cure bleeding
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            //add armor.
            AddArmor();
        }
    }


    public void TakeDamage(float damage, Transform attacker, float pushModifier = 0, int bleedingStrenght = 0, int infectionStrenght = 0)
    {
        bool alreadyPushed = false;
        if (isDead)
        {
            return;
        }
        float currentDamage = damage;
        if (currentArmor > 0)
        {
           
            if (currentArmor > currentDamage)
            {
                currentArmor -= currentDamage;
                currentDamage = 0;
                UIHolder.instance.gui.UpdateArmor(currentArmor);
            }
            else
            {
                //if current armor is the asme or less then we brake it and maybe have enough damage.
                currentDamage -= currentArmor;
                currentArmor = 0;

                UIHolder.instance.gui.BreakArmor();
            }
            if (pushModifier != 0)
            {

                int dir = -GetDir(attacker);
                StartCoroutine(handler.StunnedProcess());
                handler.rb.AddForce(new Vector2(1, 0.2f) * dir * 10, ForceMode2D.Impulse);
                alreadyPushed = true;
            }


        }

        if (currentDamage == 0) return;
        currentHealth -= currentDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (bleedingStrenght != 0) AddBleeding(bleedingStrenght);
        if (infectionStrenght != 0) AddInfection(infectionStrenght);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if(pushModifier != 0 && !alreadyPushed)
        {

            int dir = -GetDir(attacker);
            StartCoroutine(handler.StunnedProcess());
            handler.rb.AddForce(new Vector2(1,0.2f) * dir * 10, ForceMode2D.Impulse);

        }

        UIHolder.instance.gui.UpdateHealth(currentHealth, maxHealth);

    }
    public void HealHealth(float heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UIHolder.instance.gui.UpdateHealth(currentHealth, maxHealth);
    }

    public void TakeInfection(float value)
    {
        currentInfection += value;
        currentInfection = Mathf.Clamp(currentInfection, 0, maxHealth);

        if (currentInfection >= currentHealth)
        {
            //die
        }

        UIHolder.instance.gui.UpdateInfection(currentInfection, maxHealth);
    }
    public void HealInfection(float value)
    {
        currentInfection -= value;
        currentInfection = Mathf.Clamp(currentInfection, 0, maxHealth);

        UIHolder.instance.gui.UpdateInfection(currentInfection, maxHealth);
    }

    #region STATUS
    public void AddInfection(int strenght = 1)
    {
        if (!currentStatusInfection.exist)
        {
            currentStatusInfection = new StatusClass(strenght, this);
        }
        else
        {
            currentStatusInfection.UpdateStatus(1);
        }
        UIHolder.instance.gui.UpdateStatusInfection(currentStatusInfection);

    }

    public void CureInfection()
    {
        //update the ui.
        currentStatusInfection.CureStatus();       
        UIHolder.instance.gui.UpdateStatusInfection(currentStatusInfection);
        
    }


    public void AddBleeding(int strenght = 1)
    {
        if (!currentStatusBleeding.exist)
        {
            currentStatusBleeding = new StatusClass(strenght, this);
        }
        else
        {
            currentStatusBleeding.UpdateStatus(1);
        }

        UIHolder.instance.gui.UpdateStatusBleeding(currentStatusBleeding);
    }
    public void CureBleeding()
    {
        currentStatusBleeding.CureStatus();
        UIHolder.instance.gui.UpdateStatusBleeding(currentStatusInfection);
    }

    public void AddArmor()
    {
        currentArmor += 5;
        UIHolder.instance.gui.UpdateArmor(currentArmor);
    }
    #endregion


    int GetDir(Transform target)
    {
        float dir =  target.position.x - transform.position.x;

        if(dir > 0) return 1;

        if (dir < 0) return -1;

        return 0;
    }

    void Die()
    {
        isDead = true;
        handler.AddBlock("Die", PlayerHandler.BlockType.Complete);

        UIHolder.instance.death.StartDeathUI();


    }

    public bool IsDead()
    {
        return isDead;
    }

    #region SAVE
    public object CaptureState()
    {



        return new SaveData
        {
            currentHealth = currentHealth,
            currentInfection = currentInfection,
            currentArmor = currentArmor,

            infectionTick = currentStatusInfection.currentTurn,
            infectionStrenght = currentStatusInfection.strenght,

            bleedingTick = currentStatusBleeding.currentTurn,
            bleedingStrenght = currentStatusBleeding.strenght

        };
    }

    public void RestoreState(object state)
    {
        var save = (SaveData)state;

        currentHealth = save.currentHealth;
        currentInfection = save.currentInfection;
        currentArmor = save.currentArmor;

        if(save.bleedingTick > 0)
        {
            currentStatusBleeding.exist = true;
            currentStatusBleeding.currentTurn = save.bleedingTick;
            currentStatusBleeding.strenght = save.bleedingStrenght;
        }

        if(save.infectionTick > 0)
        {
            currentStatusInfection.exist = true;
            currentStatusInfection.currentTurn = save.infectionTick;
            currentStatusInfection.strenght = save.infectionStrenght;
        }

        //now we update all ui.
        gui.UpdateHealth(currentHealth, maxHealth);
        gui.UpdateInfection(currentInfection, maxHealth);
        gui.UpdateArmor(currentArmor);
        gui.UpdateStatusBleeding(currentStatusBleeding);
        gui.UpdateStatusInfection(currentStatusInfection);

    }

    [Serializable]
    struct SaveData
    {
        public float currentHealth;
        public float currentInfection;
        public float currentArmor;

        public int infectionTick;
        public int infectionStrenght;

        public int bleedingTick;
        public int bleedingStrenght;
    }

    #endregion
}

[System.Serializable]
public class StatusClass
{
    PlayerResource handler;
    //can you have buff?
    public bool exist;
    public int strenght;
    public int currentTurn;
    public int turn;
    int maxTurn = 5;


    float total = 3f;
    float current = 0;

    //we refresh the update

    public void UpdateStatus(int strenght)
    {

        if(strenght < 5) this.strenght += strenght;
        
        if(turn >= maxTurn)
        {
            //we do nothing
            currentTurn = maxTurn;
        }
        else
        {
            turn += 1;
            currentTurn = turn;
        }      

    }


    public StatusClass(int strenght, PlayerResource handler)
    {
        this.strenght = strenght;
        this.handler = handler;

        currentTurn = 3;
        turn = 3;
        exist = true;
    }

    public void CureStatus()
    {
        exist = false;

        currentTurn = 0;
        strenght = 0;
    }

    public void RunTimer()
    {
        current += Time.deltaTime;
     
    }
    public bool CanCall()
    {
        return current > total;
    }
    void BaseCall()
    {
        current = 0;
        currentTurn -= 1;
        
        

    }
    public void CallInfection()
    {
        BaseCall();

        handler.TakeInfection(strenght);
        UIHolder.instance.gui.UpdateStatusInfection(this);

        if (currentTurn <= 0)
        {
            //we need to tell teh thing to stop.
            handler.CureInfection();
        }
    }
    public void CallBleeding()
    {
        BaseCall();

        handler.TakeDamage(strenght, null);
        UIHolder.instance.gui.UpdateStatusBleeding(this);

        if (currentTurn <= 0)
        {
            //we need to tell teh thing to stop.
            handler.CureBleeding();
        }
    }

}

public enum StatusType
{
    Infection,
    Bleeding
}


public class ArmorClass
{
    //it will warn the player of the movespeed being lost.
    //it is removed in percetage.


}