using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : Actor
{
    public List<ItemDTO> items = new List<ItemDTO>();

    public List<string> itemsToSpawn = new List<string>();

    public bool isLocked = false;

    public void Interacted()
    {
        MasMan.InventoryManager.loot.Interacted(this);

    }



    void Start()
    {
        MoveTo(new NesScripts.Controls.PathFind.Point(startingPosition.x, startingPosition.y));

        items = new List<ItemDTO>(MasMan.InventoryManager.loot.slots.Count);

        for (int i = 0; i < MasMan.InventoryManager.loot.slots.Count; i++)
        {

            if (itemsToSpawn.Count > i)
                items.Add(ItemDTO.CreateItem(itemsToSpawn[i]));
            else
                items.Add(ItemDTO.EmptyItem());
        }
    }

    public override void Update()
    {
        base.Update();

        if (isTurn && hasInput)
        {
            CompleteTurn(10, 0);
        }
    }
}
