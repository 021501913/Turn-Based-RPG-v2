using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Slot : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public ItemDTO item;

    public bool hovered = false;

    public string startingItem = "EmptyItem";

    public TextMeshProUGUI label;

    public Image spriteImage;

    public float hoverTimer = 0;

    public void Awake()
    {
        item = ItemDTO.CreateItem(startingItem);
    }

    public void Update()
    {
        if (hovered)
        {
            hoverTimer += Time.deltaTime;
            if (hoverTimer > 0.25f && hoverTimer < 50 && item.exists)
            {
                hoverTimer = 50;
                ItemTooltip.ItemToolti.ShowTooltip();
                ItemTooltip.ItemToolti.ApplyTooltip(item);
            }

            if (Input.GetMouseButtonDown(0) && item.exists)
            {
                inventoryManager.PickupItem(this);
            }
            else if (Input.GetMouseButtonUp(0) && inventoryManager.pickedUpItem)
            {
                if (item.exists)
                {
                    inventoryManager.SwapItems(this);
                }
                else
                    inventoryManager.PlaceItem(this);
            }
        }

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        label.SetText(item.label);
        spriteImage.sprite = item.sprite;
    }

    public void OnHover()
    {
        hoverTimer = 0;



        hovered = true;
        //Debug.Log("hover on");
    }

    public void OffHover()
    {
        hoverTimer = 0;

        ItemTooltip.ItemToolti.HideTooltip();
        hovered = false;
        //Debug.Log("hover off");
    }
}
