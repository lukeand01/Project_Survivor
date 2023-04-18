using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyBase : MonoBehaviour, IDamageable
{

    public EnemyData data;
    protected bool isAttackCooldown;
    protected bool isDead;
    
    protected Rigidbody2D rb;
    protected BoxCollider2D myCollider;

    float currentHealth;
    [SerializeField] Transform feet;
    [SerializeField] Transform face;
    [SerializeField] Transform gapChecker;

    [SerializeField] List<ItemClass> inventoryList = new List<ItemClass>(); 

    bool isStunned;

    [Separator("DEBUG")]
    public bool DEBUG_CANNOTMOVE;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        currentHealth = data.enemyHealth;
    }


    protected bool IsTargetInDetectionRange(Transform target, float range = 0)
    {
        float distance = Vector3.Distance(target.position, transform.position);
        return data.enemyDetectRange + range >= distance;
    }



    protected bool IsTargetInAttackRange(Transform target, float range = 0 )
    {
        float distance = Vector3.Distance(target.position, transform.position);
        return data.enemyAttackRange + range >= distance;
    }


    protected bool ShouldFall()
    {
        if (isStunned) return true;

        bool check = Physics2D.Raycast(feet.position, -Vector2.up, 0.15f, LayerMask.GetMask("Floor"));

        if (check)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            return false;
        }
        else
        {

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            return true;
        }

        
    }
    int patrolDir = 1;
    protected int currentDir = 1;
    bool IsTurnCooldown;
    protected void Patrol()
    {
        if (IsWallAhead() && !IsTurnCooldown || IsGapAhead() && !IsTurnCooldown)
        {
            //change direction
            patrolDir *= -1;
            IsTurnCooldown = true;
            Invoke(nameof(TurnCooldownOrder), 0.1f);
        }

        //move towards the direction
        Move(patrolDir);
    }

    void TurnCooldownOrder() => IsTurnCooldown = false;


    protected virtual bool IsWallAhead()
    {
        bool check = Physics2D.Raycast(face.position, Vector2.right, 0.3f, LayerMask.GetMask("Floor"));
        return check;
    }

    bool IsGapAhead()
    {
        bool check = Physics2D.Raycast(gapChecker.position, -Vector2.up, 0.3f, LayerMask.GetMask("Floor"));
        if (check)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    //should we aim to jump at stuff?
    //i want to be able to push them. but i want to be able to dash through them.

    protected void Move(int dir)
    {
        rb.velocity = new Vector2(dir * data.enemySpeed, rb.velocity.y);
        Rotate(dir);
    }
    void Rotate(int dir)
    {
       if(dir != 0)currentDir = dir;
        if(dir == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if(dir == -1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    protected int GetPlayerDir()
    {
        float playerX = PlayerHandler.instance.transform.position.x;
        if(playerX > transform.position.x)
        {
            return 1;
        }
        if(playerX < transform.position.x)
        {
            return -1;
        }
        return 0;

    }

    protected int GetTargetDir(Transform target)
    {
        float playerX = target.position.x;
        if (playerX > transform.position.x)
        {
            return 1;
        }
        if (playerX < transform.position.x)
        {
            return -1;
        }
        return 0;
    }

    public void TakeDamage(float damage, Transform attacker, float pushModifier = 1, int bleedingStrenght = 0, int infectionStrenght = 0)
    {
        //all damage throws teh fellas back.

        if (isDead)
        {
            Debug.Log("its dead");
            return;
        }

        currentHealth -= damage;

        if (pushModifier != 0)
        {
            int dir = -GetDir(attacker);
            StartCoroutine(StunnedProcess());
            rb.AddForce(new Vector2(1, 0.2f) * dir * 10, ForceMode2D.Impulse);

        }

        if (currentHealth <= 0) Die();
    }

    IEnumerator StunnedProcess()
    {
        isStunned = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        myCollider.isTrigger = false;

        yield return new WaitForSeconds(0.15f);

        rb.velocity = new Vector2(0, 0);

        yield return new WaitForSeconds(0.2f);
        isStunned = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        myCollider.isTrigger = true;
        


    }


    int GetDir(Transform target)
    {
        float dir = target.position.x - transform.position.x;

        if (dir > 0) return 1;

        if (dir < 0) return -1;

        return 0;
    }

    void Die()
    {
        isDead = true;

        for (int i = 0; i < inventoryList.Count; i++)
        {
           PlayerHandler.instance.inventory.ReceiveItem(inventoryList[i]);
        }
        

    }

    protected Transform GetHuman()
    {
        //we find it through the tag.
        //we shoot a raycast to both sides.

        RaycastHit2D[] rayList = Physics2D.CircleCastAll(transform.position, data.enemyDetectRange, Vector2.zero,10);

        if (rayList.Length <= 0) return null;

        float closestTarget = 0;
        Transform currentTarget = null;

        for (int i = 0; i < rayList.Length; i++)
        {
            
            if (rayList[i].transform.tag != "Human")
            {
                continue;
            }
            if (DifferenceInYTooHigh(rayList[i].transform, 5))
            {
                continue;
            }
            IDamageable damageble = rayList[i].collider.gameObject.GetComponent<IDamageable>();
            
            if (damageble.IsDead()) continue;


                float distance = Vector3.Distance(rayList[i].transform.position, transform.position);
            if (currentTarget == null)
            {
                closestTarget = distance;
                currentTarget = rayList[i].transform;
            }
            else
            {
                if(closestTarget > distance)
                {
                    closestTarget = distance;
                    currentTarget = rayList[i].transform;
                }
            }

        }


        return currentTarget;
    }

    protected bool DifferenceInYTooHigh(Transform target, float max)
    {
        float differenceY = Mathf.Abs(target.position.y - transform.position.y);

        return differenceY > max;

    }

    protected Transform GetThreat()
    {
        //get a treat based on its prefenrces.
        return null;
    }

    int maxTick = 5;
    int currentTick = 0;
    public bool IsTick()
    {
        if(currentTick >= maxTick)
        {
            currentTick = 0;
            return true;
        }
        else
        {
            currentTick += 1;
            return false;
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

   

    //humans react differently to zombies and players.

    //two types of behavior:
    //survivor : it will warn the player to not get close.
    //bandit : it will target the closest target. and will try to chase the player.

}
