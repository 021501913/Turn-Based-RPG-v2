using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockpickItem : ItemDTO
{
    public override void Init()
    {
        mgDTO = new MGDTO()
        {
            damageMin = 0,
            damageMax = 0,
            type = "WORK",
            frequency = 3,
            maxScore = 3,
            duration = 0,
            actionCost = 0,
        };

        label = "Lockpick";
        actionCost = 0;
        exists = true;
        flavour = "A simple set of lockpicks, they break easily.";
        //description = "Allows unlocking of chests.";
        amount = 3;
        maxAmount = 3;
        type = Type.misc;

        base.Init();
    }

    public override void Activate()
    {
        //MiniGameManager.MiniGameManage.MGArcheryAttack.StartMiniGame(P, Actor.actors[i], 20, 50, 0, "ATTACK");
    }

    public override void Attack(Actor target)
    {
        mgDTO.actorInitiated = Actor.player;
        mgDTO.actorTargeted = target;

        MasMan.MGMan.MGLockpick.StartMiniGame(mgDTO);
    }
}
