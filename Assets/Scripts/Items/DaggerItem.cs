using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerItem : ItemDTO
{
    public override void Init()
    {
        mgDTO = new MGDTO()
        {
            damageMin = 15,
            damageMax = 30,
            type = "ATTACK",
            frequency = 3,
            maxScore = 12,
            duration = 3,
            actionCost = 20,
        };

        label = "Dagger";
        exists = true;
        type = Type.weapon;
        flavour = "Nifty little dagger";
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

        MasMan.MGMan.MGDaggerAttack.StartMiniGame(mgDTO);
    }
}
