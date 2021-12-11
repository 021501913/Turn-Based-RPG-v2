using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImage : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public bool hovering = false;

    private void Update()
    {
        if (hovering)
        {


            if (Input.GetMouseButtonUp(0) && inventoryManager.pickedUpItem != null)
            {
                if (inventoryManager.pickedUpItem.item.type == ItemDTO.Type.consumable && Actor.player.isTurn)
                {
                    inventoryManager.pickedUpItem.item.Activate();
                    inventoryManager.pickedUpItem.item = ItemDTO.EmptyItem();
                }
                
                inventoryManager.ClearDraggedItem();
            }

        }
    }

    public void OnHover()
    {
        hovering = true;
    }

    public void OffHover()
    {
        hovering = false;
    }
}
