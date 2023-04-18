using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] InventoryUnit template;
    [SerializeField] Transform itemContainer;
    [SerializeField] Transform notificationContainer;
    [SerializeField] Image loadingImage;

    GameObject holder;
    InventoryUnit currentUnit;

    List<ItemClass> uiInventoryList = new List<ItemClass>();

    private void Awake()
    {
        holder = transform.GetChild(1).gameObject;
    }


    float baseActionTimer = 0.00003f;
    float currentActionTimer;

    bool isHoldLoadingImage;

    private void Update()
    {
        if (isHoldLoadingImage) return;
        if (Input.GetMouseButtonUp(0))
        {
            //remove it.
            if(currentUnit != null)
            {
                loadingImage.gameObject.SetActive(false);
                currentActionTimer = 0;
                loadingImage.fillAmount = currentActionTimer / baseActionTimer;
            }
        }
        if (Input.GetMouseButton(0))
        {
            
            if(currentUnit != null)
            {
                loadingImage.transform.position = Input.mousePosition;
                currentActionTimer += Time.deltaTime;
                loadingImage.gameObject.SetActive(true);
                if (currentActionTimer > baseActionTimer)
                {
                    //interact with the unit and the inventory at the same time.
                    currentUnit.item.data.Act();
                    currentActionTimer = 0;                   
                    currentUnit.item.RemoveQuantity();
                    currentUnit.UpdateUI();
                    PlayerHandler.instance.inventory.CheckThisSide(currentUnit.index);
                    StartCoroutine(LoadingImageProcess());

                    if (!currentUnit.item.ShouldExist())
                    {
                        //we do nothing.
                        Destroy(currentUnit.gameObject);
                        currentUnit = null;                       
                        //update everyone after this unit by lowering the index by one.
                        UpdateRightIndex();
                        Observer.instance.OnGlobalDescriptor("");


                    }                   
                }

                loadingImage.fillAmount = currentActionTimer / baseActionTimer;

                //it stays a bit up in teh thing.
            }
        }
    }

    List<InventoryUnit> notificationList = new List<InventoryUnit>();
    public void AddNotification(ItemClass item)
    {
        int index = GetNotificationIndex(item);

        if(index == -1)
        {
            //add a new one
            InventoryUnit newObject = Instantiate(template, notificationContainer.position, Quaternion.identity);
            newObject.transform.parent = notificationContainer;
            newObject.NotificationSetUp(item, 2.5f, this);
            notificationList.Add(newObject);
        }
        else
        {
            //add.
            notificationList[index].NotificationAdd(item.quantity);
        }

    }
    public void RemoveNotification(ItemClass item)
    {
        int index = GetNotificationIndex(item);

        if(index == -1)
        {
            //there is no one here and something wrong happened
            Debug.Log("remove notification failed");
        }
        else
        {
            Destroy(notificationList[index].gameObject);
            notificationList.RemoveAt(index);
        }
    }
    int GetNotificationIndex(ItemClass item)
    {
        for (int i = 0; i < notificationList.Count; i++)
        {
            if (notificationList[i].item.data == item.data) return i;

        }

        return -1;
    }


    void UpdateRightIndex()
    {

        for (int i = 0; i < itemContainer.childCount; i++)
        {
            itemContainer.transform.GetChild(i).GetComponent<InventoryUnit>().index = i - 1;
        }
    }

    //do i want that interaction or do i preff a little bar above?
    //if you close the inventory
    IEnumerator LoadingImageProcess()
    {
        isHoldLoadingImage = true;



        yield return new WaitForSeconds(0.2f);

        isHoldLoadingImage = false;
    }


    public void ControlUI()
    {

        if (holder.activeInHierarchy)
        {
            holder.SetActive(false);
            PlayerHandler.instance.RemoveBlock("Inventory");
        }
        else
        {
            holder.SetActive(true);
            PlayerHandler.instance.AddBlock("Inventory", PlayerHandler.BlockType.Partial);
        }
    }

    public void UpdateUI(List<ItemClass> inventoryList)
    {

        ClearContainer();
        for (int i = 0; i < inventoryList.Count; i++)
        {
            CreateUnit(inventoryList[i], i);
        }
        uiInventoryList = inventoryList;

    }
    void CreateUnit(ItemClass item, int index)
    {
        InventoryUnit unit = Instantiate(template, itemContainer.position, Quaternion.identity);
        unit.SetUp(item, this, index);
        unit.transform.parent = itemContainer.transform;
    }

    void ClearContainer()
    {
        for (int i = 0; i < itemContainer.childCount; i++)
        {
            Destroy(itemContainer.transform.GetChild(i).gameObject);
        }
    }

    public void PressUnit(InventoryUnit unit)
    {
        currentUnit = unit;
        if (unit == null)
        {
            loadingImage.gameObject.SetActive(false);
            return;
        }
    }

}
