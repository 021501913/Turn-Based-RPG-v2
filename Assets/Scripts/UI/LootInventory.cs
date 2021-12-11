using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInventory : MonoBehaviour
{

    public List<Slot> slots = new List<Slot>();

    public bool open = false;

    Loot currentLootActor;

    public void Interacted(Loot _currentLootActor)
    {
        if (open)
        {
            CloseLoot();
        }
        else if (!open)
        {
            OpenLoot(_currentLootActor);
        }
    }

    public void OpenLoot(Loot _currentLootActor)
    {
        currentLootActor = _currentLootActor;
        open = true;



        MasMan.InventoryManager.loot.gameObject.SetActive(true);

        for (int i = 0; i < MasMan.InventoryManager.loot.slots.Count; i++)
        {
            if (currentLootActor.items.Count > i)
            {
                MasMan.InventoryManager.loot.slots[i].item = currentLootActor.items[i];

            }
        }


    }

    public void CloseLoot()
    {
        open = false;

        MasMan.InventoryManager.loot.gameObject.SetActive(false);

        for (int i = 0; i < currentLootActor.items.Count; i++)
        {
            currentLootActor.items[i] = MasMan.InventoryManager.loot.slots[i].item;
        }
    }

    private void Awake()
    {
    }

}
