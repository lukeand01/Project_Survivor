using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerHandler handler;

    private void Awake()
    {
        handler = GetComponent<PlayerHandler>();
    }

    private void Update()
    {
        if (handler.HasBlock(PlayerHandler.BlockType.Complete)) return;

        PauseInput();

        if (handler.HasBlock(PlayerHandler.BlockType.SecondPartial)) return;

        InventoryInput();

        if (handler.HasBlock(PlayerHandler.BlockType.Partial)) return;

       
        MoveInput();
        JumpInput();
        LookInput();
        DashInput();
        InteractInput();
        CombatInput();
    }

    void CombatInput()
    {
        if (Input.GetKeyDown(handler.GetKey("Melee"))) 
        {
            handler.combat.UseMelee();
            handler.combat.StartGun(false);
            return;
        }

        if (Input.GetKey(handler.GetKey("Gun")))
        {
            handler.combat.StartGun(true);
            
            return;
        }

        handler.combat.StartGun(false);
        
    }

    void MoveInput()
    {

        //if i am flying from climb then we dont look at getkey only getkey down.
        if (handler.move.cannotSideMove)
        {
            if (Input.GetKeyDown(handler.GetKey("MoveLeft")) && Input.GetKeyDown(handler.GetKey("MoveRight")))
            {
                Debug.Log("can move sideways");
                handler.move.cannotSideMove = false;
                
            }
            
            return;
        }

        int dir = 0;

        if (Input.GetKey(handler.GetKey("MoveLeft")))
        {
            dir = -1;
        }
        if (Input.GetKey(handler.GetKey("MoveRight")))
        {
            dir = 1;
        }

        handler.move.Move(dir);
    }

    void LookInput()
    {
        //we look down. if we flying then we just increase gravity.

        if (Input.GetKey(handler.GetKey("MoveDown")))
        {
            //if we are not grounded. we go down faster.

            //if we are grounded then we look down,
            Debug.Log("press it");
            if (handler.isGrounded())
            {
                handler.cam.ControlCameraVertical(-1);
            }
            else
            {

            }
            return;
        }

        if (Input.GetKey(handler.GetKey("MoveUp")))
        {
            //just look up. but only do so when you are grounded.
            if (handler.isGrounded())
            {
                handler.cam.ControlCameraVertical(1);
            }

            return;
        }

        handler.cam.ControlCameraVertical(0);

    }

    void ClimbInput()
    {
        //
        
    }

    void JumpInput()
    {      
        //


        if (Input.GetKeyDown(handler.GetKey("Jump")))
        {
            handler.move.PressInput();
        }
        if (Input.GetKey(handler.GetKey("Jump")))
        {
            handler.move.HoldInput();
        }
        if (Input.GetKeyUp(handler.GetKey("Jump")))
        {
            //when you release the key 
            handler.move.ReleaseInput();
        }

    }

    void InventoryInput()
    {
        if (Input.GetKeyDown(handler.GetKey("Inventory")))
        {
            UIHolder.instance.inventory.ControlUI();
        }
    }

    void PauseInput()
    {
        if (Input.GetKeyDown(handler.GetKey("Pause")))
        {
            UIHolder.instance.menu.ControlUI();
        }
    }

    void DashInput()
    {
        if (Input.GetKeyDown(handler.GetKey("Dash"))) handler.move.Dash();
    }
    
    void InteractInput()
    {

        if (Input.GetKey(handler.GetKey("Interact")))
        {
            handler.HandleInteraction(true);
        }
        else
        {
            handler.HandleInteraction(false);
        }
    }
}
