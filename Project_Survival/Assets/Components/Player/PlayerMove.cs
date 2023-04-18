using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    PlayerHandler handler;

    [SerializeField] float currentSpeed;
    int lastDir = 1;



    private void Awake()
    {
        lastDir = 1;
        handler = GetComponent<PlayerHandler>();
        handler.EventClimb += ResetMomentum;
    }


    bool isSliding;
    public bool cannotSideMove;

    private void Update()
    {



        if (handler.isGrounded())
        {
            currentCooldown = 0;
            
        }

        if (handler.isGrounded())
        {
            handler.rb.gravityScale = 1.2f;
            bonusMovement = 0;
            jumpedFromSlide = false;
            isDashGrounded = false;
        }

       
        if (handler.isFalling())
        {
            //put it to a limit.
            float maxFallVelocity = handler.rb.velocity.y;
            maxFallVelocity = Mathf.Clamp(maxFallVelocity, -10, 0);
            handler.rb.velocity = new Vector3(handler.rb.velocity.x, maxFallVelocity, 0);
        }

        if (isDashCooldown)
        {
            if(currentDashCooldown > baseDashCooldown)
            {
                isDashCooldown = false;
                currentDashCooldown = 0;
            }
            else
            {
                currentDashCooldown += Time.deltaTime;
            }
            UIHolder.instance.gui.UpdateDashCooldown(currentDashCooldown, baseDashCooldown);

        }
        
    }

    float baseSlideJumpCooldown = 0.18f;//this is the grace time for when you are sliding.
    float currentSlideJumpCooldown;

    public bool jumpedFromSlide;
    

    [SerializeField]float currentIncreasingMoveSpeed;
    [SerializeField] float topIncreasingMoveSpeed;
    [SerializeField]bool isMomentum;

    public float isGunMoveDebuff = 0;
    public float isGunJumpDebuff = 0;

    //the camera does not follow the momentum

    void MomentumOrder() => isMomentum = false;

    


    public void Move(int dir)
    {
        //the problem is the last dir
        if (isMomentum)
        {
            return;
        }
        if (isDashing) return;
        if (cannotSideMove) return;

        //speed increases right here.

        CalculateIncreasingMoveSpeed(dir);



        if (dir != 0) lastDir = dir;
        if (dir != 0 && handler.IsClimbing(dir) && !handler.isGrounded())
        {

            //slowly fall behind.
            handler.OnClimb();
            handler.rb.velocity = new Vector2(0, -0.4f);
            isSliding = true;
            dir = 0;
            //can jump

        }
        else
        {

            if (isSliding)
            {
                if (currentSlideJumpCooldown > baseSlideJumpCooldown)
                {
                    isSliding = false;
                    currentSlideJumpCooldown = 0;
                }
                else
                {
                    currentSlideJumpCooldown += Time.deltaTime;
                }
            }

        }


        if (jumpedFromSlide)
        {
            //then we take physics more into account         
            handler.rb.AddForce((Vector2.right * dir) * Time.deltaTime, ForceMode2D.Force);

        }
        else
        {

            handler.rb.velocity = new Vector2(dir * (currentSpeed + bonusMovement + currentIncreasingMoveSpeed - isGunMoveDebuff), handler.rb.velocity.y);
        }

            
        handler.cam.ControlCameraHorizontal(dir);

        //if there is a wall ahead
        if(!jumpedFromSlide && !handler.combat.isUsingGun)
        {
            Rotate(dir);

        }

    }

    void ResetMomentum()
    {
        currentIncreasingMoveSpeed = 0;
    }

    void CalculateIncreasingMoveSpeed(int dir)
    {

        //if we click we lose that velocity.

        //if you are holding this thing you cannot move faster.
        if (handler.combat.isUsingGun)
        {
            currentIncreasingMoveSpeed = 0;
            return;
        }

        if (!handler.isGrounded())
        {
            //we do not increase but we also dont take.
            return;
        }

        if (dir != 0 && dir == lastDir)
        {
            //increases.

            currentIncreasingMoveSpeed += Time.deltaTime * 5;
        }

        if (dir == 0 || dir != lastDir)
        {
            if (currentIncreasingMoveSpeed > topIncreasingMoveSpeed / 2)
            {
                //if its more than half then we have 
                handler.rb.AddForce((Vector2.right * lastDir * 10), ForceMode2D.Force);
                isMomentum = true;
                Invoke(nameof(MomentumOrder), 0.2f);
            }

            currentIncreasingMoveSpeed = 0;
        }
    }

    void HandleClimb(int dir)
    {

    }

    public void Rotate(int dir)
    {
        if(dir == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if(dir == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void Jump(float modifier)
    {
        handler.rb.velocity = new Vector2(handler.rb.velocity.x, modifier);
    }

    bool isDashCooldown;
    bool isDashing;
    bool isDashGrounded;
    float baseDashCooldown = 0.5f;
    float currentDashCooldown;

    #region DASH
    public void Dash()
    {
        if (isDashCooldown)
        {
            Debug.Log("dash cooldown");
            return;
        }

        if (isDashGrounded) return;

        gameObject.layer = 8;

        handler.rb.AddForce(Vector2.right * lastDir * 20, ForceMode2D.Impulse);
        isDashCooldown = true;
        isDashing = true;
        isDashGrounded = true;
        Invoke(nameof(DashingOrder), 0.20f);
        StartCoroutine(handler.ImmunityProcess());
    }

    void DashingOrder()
    {
        gameObject.layer = 7;
        isDashing = false;
    }

    #endregion


    #region JUMP
    float baseCooldown = 0.39f;
    float currentCooldown;
   public bool IsHoldingJump = false;

    float bonusMovement = 0;

    public void HoldInput()
    {
        if(!IsHoldingJump)
        {
            return;
        }
        if (currentCooldown > baseCooldown)
        {
            if (IsHoldingJump)
            {
                IsHoldingJump = true;
                handler.rb.gravityScale = 1.9f;
            }
            
            return;
        }


        //in the peak of holding we get a small speed forward.
        if(currentCooldown > 0.1f)
        {
            bonusMovement = 1;
        }

        currentCooldown += Time.deltaTime;
        Jump(8.7f - (isGunJumpDebuff * 3));

    }
    public void PressInput()
    {
        if (isSliding)
        {
            //
            //jump against the last dir.
            cannotSideMove = true;
            jumpedFromSlide = true;
            Invoke(nameof(CannotSideJumpOrder), 0.2f);
            Invoke(nameof(JumpFromSlideOrder), 0.4f);
            Jump(2f);
            Vector3 dir = (Vector2.right * (lastDir * -1) * 8) + Vector2.up * 7;
            handler.rb.AddForce(dir , ForceMode2D.Impulse);
            Rotate(lastDir * -1);
            //handler.rb.velocity = new Vector2((lastDir * -1) * 15, handler.rb.velocity.y);
            currentCooldown = 0;

            return;
        }


        if (!handler.isGrounded())
        {
            return;
        }
        Jump(5.5f - isGunJumpDebuff);
        IsHoldingJump = true;
        
    }  
    public void ReleaseInput()
    {
        handler.rb.gravityScale = 2.5f;
        IsHoldingJump = false;
    }
    void CannotSideJumpOrder()
    {
        cannotSideMove = false;
    }
    void JumpFromSlideOrder() => jumpedFromSlide = false;
    //the problem is when i start falling.

    #endregion
}
