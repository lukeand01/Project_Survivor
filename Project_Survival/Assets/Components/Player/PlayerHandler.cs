using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour, ISaveable
{
    public static PlayerHandler instance;
    [HideInInspector] public PlayerController controller;
    [HideInInspector] public PlayerMove move;
    [HideInInspector] public PlayerInventory inventory;
    [HideInInspector] public PlayerResource resource;
    [HideInInspector] public PlayerCamera cam;
    [HideInInspector] public PlayerCombat combat;
    [HideInInspector] public Rigidbody2D rb;
    

    [Separator("PIECES")]
    [SerializeField] Transform feet;
    [SerializeField] Transform upFace;
    [SerializeField] Transform downFace;
   
    [SerializeField] GameObject body;
    SpriteRenderer bodyRend;

    [Separator("COLOR")]
    [SerializeField] Color immunityColor;
    Color originalColor;


    public bool hasSledgehammer;
    public bool IsWarned;
    

    private void Awake()
    {
        bodyRend = body.GetComponent<SpriteRenderer>();
        originalColor = bodyRend.color;

        rb = GetComponent<Rigidbody2D>();

        controller = GetComponent<PlayerController>();
        move = GetComponent<PlayerMove>();
        inventory = GetComponent<PlayerInventory>();
        resource = GetComponent<PlayerResource>();
        combat = GetComponent<PlayerCombat>();

        cam = Camera.main.gameObject.GetComponent<PlayerCamera>();

        SetUpKeys();
        instance = this;

        
    }

    
    public void ControlTimer(float timer)
    {
        Time.timeScale = timer;

    }



    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SaveHandler.instance.Save("First");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveHandler.instance.Load("First");
        }


    }

    #region KEYCODES
    KeyCode keyMoveLeft;
    KeyCode keyMoveRight;
    KeyCode keyMoveUp;
    KeyCode keyMoveDown;
    KeyCode keyJump;
    KeyCode keyPause;
    KeyCode keyDash;
    KeyCode keyInteract;
    KeyCode keyInventory;
    KeyCode keyGun;
    KeyCode keyMelee;
    public KeyCode GetKey(string id)
    {
        switch (id)
        {
            case "MoveLeft":
                return keyMoveLeft;
            case "MoveRight":
                return keyMoveRight;
            case "MoveUp":
                return keyMoveUp;
            case "MoveDown":
                return keyMoveDown;
            case "Jump":
                return keyJump;
            case "Pause":
                return keyPause;
            case "Dash":
                return keyDash;
            case "Interact":
                return keyInteract;
            case "Inventory":
                return keyInventory;
            case "Gun":
                return keyGun;
            case "Melee":
                return keyMelee;
        }

        return KeyCode.None;
    }

    void ChangeKey(string id, KeyCode key)
    {

    }

    void SetUpKeys()
    {
        keyMoveLeft = KeyCode.A;
        keyMoveRight = KeyCode.D;
        keyMoveDown = KeyCode.S;
        keyMoveUp = KeyCode.W;

        keyPause = KeyCode.Escape;

        keyJump = KeyCode.Space;
        keyDash = KeyCode.LeftShift;

        keyInteract = KeyCode.E;

        keyInventory = KeyCode.Tab;

        keyMelee = KeyCode.F;
        keyGun = KeyCode.Q;
    }
    #endregion

    #region BLOCKS

    //get blocks to stop movement.
    public Dictionary<string, BlockType> blockNary = new Dictionary<string, BlockType>();

    public void AddBlock(string id, BlockType block)
    {
        if (blockNary.ContainsKey(id))
        {
            //if we already have that key then we dont.
            return;
        }

        if(id == "Pause")
        {
            ControlTimer(0);
        }

        if(id == "Inventory")
        {
            ControlTimer(Time.deltaTime * 0.01f);
        }


        blockNary.Add(id, block);
    }
    public bool HasBlock(BlockType block)
    {
        return blockNary.ContainsValue(block);
    }

    public bool HasBlockID(string id)
    {
        return blockNary.ContainsKey(id);
    }

    public void RemoveBlock(string id)
    {
        if (blockNary.ContainsKey(id))
        {
            blockNary.Remove(id);
        }

        if (HasBlockID("Pause"))
        {
            ControlTimer(0);
            return;
        }
        if (HasBlockID("Inventory"))
        {
            ControlTimer(Time.deltaTime * 0.01f);
            return;
        }
        ControlTimer(1);
    }

    public void ClearBlock()
    {
        blockNary.Clear();
    }

    public enum BlockType
    {
        Complete,
        Partial,
        Mouse,
        Movement,
        Input,
        Interact,
        SecondPartial


    }

    #endregion

    #region EVENTS
    //events of certains things for trigger passives.
    public event Action eventTakeDamage;
    public void OnTakeDamage() => eventTakeDamage?.Invoke();

    public event Action eventPlayerDeath;
    public void OnPlayerDeath() => eventPlayerDeath?.Invoke();

    public event Action eventUseSpell;
    public void OnUseSpell() => eventUseSpell?.Invoke();


    public event Action eventAttack;
    public void OnAttack() => eventAttack?.Invoke();


    public event Action eventStomp;
    public void OnStomp() => eventStomp?.Invoke();

    public event Action EventClimb;
    public void OnClimb() => EventClimb?.Invoke();
    #endregion

    #region CHECKS
    public bool isGrounded()
    {
        return Physics2D.OverlapCircle(feet.transform.position, 0.1f, LayerMask.GetMask("Floor"));
    }

    
    public bool isFalling()
    {
        return rb.velocity.y < -2;
    }

    public bool isWallAhead(int dir, float rangeModifier = 0.3f)
    {
        return Physics2D.Raycast(upFace.position, Vector2.right * dir, rangeModifier, LayerMask.GetMask("Floor"));
    }

    public bool IsClimbing(int dir)
    {
        bool checkUp = Physics2D.Raycast(upFace.position, Vector2.right * dir, 0.1f, LayerMask.GetMask("Floor"));
        bool checkDown = Physics2D.Raycast(downFace.position, Vector2.right * dir, 0.1f, LayerMask.GetMask("Floor"));

        if (checkDown || checkUp) return true;

        return false;
    }

    #endregion
    #region SAVE 
    public object CaptureState()
    {

        return new SaveData
        {
            posX = transform.position.x,
            posY = transform.position.y,
            hasSledgehammer = hasSledgehammer               
        };
    }

    public void RestoreState(object state)
    {
        var save = (SaveData)state;

        transform.position = new Vector2(save.posX, save.posY);
        hasSledgehammer = save.hasSledgehammer;
    }

    [Serializable]
    struct SaveData
    {
        public float posX;
        public float posY;
        public bool hasSledgehammer;
    }

    #endregion


    public void ReceiveItem(ItemClass item) => inventory.ReceiveItem(item);

    public IEnumerator StunnedProcess()
    {
        gameObject.layer = 8;
        bodyRend.color = immunityColor;
        AddBlock("Stun", BlockType.Partial);
        yield return new WaitForSeconds(0.15f);
        RemoveBlock("Stun");
        yield return new WaitForSeconds(0.15f);
        gameObject.layer = 7;
        bodyRend.color = originalColor;
    }

    public IEnumerator ImmunityProcess()
    {
        gameObject.layer = 8;
        bodyRend.color = immunityColor;
        yield return new WaitForSeconds(0.2f);
        gameObject.layer = 7;
        bodyRend.color = originalColor;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "Zombie") return;

        
    }

    public void HandleInteraction(bool isInteracting)
    {
        if (currentInteract == null) return;
        //there are two types of interactions.
        currentInteract.Interact(isInteracting);

        if (!currentInteract.IsInteractable()) currentInteract = null;
        
    }

    IInteractable currentInteract;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interact = collision.GetComponent<IInteractable>();

        if (interact != null)
        {
            if (!interact.IsInteractable()) return;
            currentInteract = interact;
            interact.InteractUI(true);

        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable interact = collision.GetComponent<IInteractable>();

        if (interact != null)
        {
            if (!interact.IsInteractable()) return;
            currentInteract = interact;
            interact.InteractUI(false);
        }
    }

   
}
