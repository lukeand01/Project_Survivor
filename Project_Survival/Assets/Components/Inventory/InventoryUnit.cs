using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUnit : ButtonBase
{
    public ItemClass item;

    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] Image portrait;
    InventoryUI handler;
    public int index;

    bool cannotInteract;

    private void Update()
    {
        if (totalTimer <= 0) return;

        NotificationHandle();

    }

  

    public void SetUp(ItemClass item, InventoryUI handler, int index)
    {
        this.item = item;
        this.handler = handler;
        this.index = index;
        UpdateUI();
    }

    float currentTimer;
    float totalTimer;
    float alphaVariable = 0;

    public void NotificationSetUp(ItemClass item, float timer, InventoryUI handler)
    {
        this.item = item;
        totalTimer = timer;
        this.handler = handler;
        alphaVariable = (timer / 2) / 1000;
        UpdateUI();
    }

    public void NotificationAdd(int quantity)
    {
        currentTimer = 0;
        item.IncreaseQuantity(quantity);
        SetFadeEffect(1);
        UpdateUI();
    }

    void NotificationHandle()
    {
        if(currentTimer > totalTimer)
        {
            //remove the thing from the handle.
            handler.RemoveNotification(item);
        }
        else
        {
            //when it gets to a certain value we start removing from the alpha as well.
            HandleEffect();
            currentTimer += Time.deltaTime;
        }
    }

    void HandleEffect()
    {
        if (currentTimer < totalTimer / 2) return;

        ChangeFadeEffect(-alphaVariable);

    }

    void SetFadeEffect(float value)
    {
        var backgroundColor = background.color;
        backgroundColor.a = value;
        background.color = backgroundColor;

        var titleColor = titleText.color;
        titleColor.a = value;
        titleText.color = titleColor;

        var quantityColor = quantityText.color;
        quantityColor.a = value;
        quantityText.color = quantityColor;

        var portraitColor = portrait.color;
        portraitColor.a = value;
        portrait.color = portraitColor;
    }

    void ChangeFadeEffect(float value)
    {
        var backgroundColor = background.color;
        backgroundColor.a += value;
        background.color = backgroundColor;

        var titleColor = titleText.color;
        titleColor.a += value;
        titleText.color = titleColor;

        var quantityColor = quantityText.color;
        quantityColor.a += value;
        quantityText.color = quantityColor;

        var portraitColor = portrait.color;
        portraitColor.a += value;
        portrait.color = portraitColor;
    }


    public void UpdateUI()
    {
        titleText.text = item.data.itemName;
        quantityText.text = item.quantity.ToString();
        portrait.sprite = item.data.itemPortrait;
    }
  

    public override void OnPointerEnter(PointerEventData eventData)
    {
        //create description
        if (totalTimer > 0) return;
        if (item == null) return;
        if (item.data == null) return;
        handler.PressUnit(this);
        Observer.instance.OnGlobalDescriptor(item.data.itemDescription);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (totalTimer > 0) return;
        if (item == null) return;
        if (item.data == null) return;
        handler.PressUnit(null);
        Observer.instance.OnGlobalDescriptor("");
    }

}
