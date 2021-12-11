using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AppleItem : ItemDTO
{
    public override void Init()
    {
        label = "Apple";
        actionCost = 50;
        exists = true;
        type = Type.consumable;
        flavour = "An apple, what do you want from me?";
        description = "+ 20 Health";
        base.Init();
    }

    public override void Activate()
    {
        Player.player.Heal(20);
        Player.player.CompleteTurn(actionCost, 0);
    }
}
