using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowItem : ItemDTO
{
    public override void Init()
    {
        mgDTO = new MGDTO()
        {
            damageMin = 15,
            damageMax = 30,
            type = "ATTACK",
            frequency = 3,
            maxScore = 15,
            duration = 2,
            actionCost = 30,
        };

        label = "Bow";
        actionCost = 20;
        exists = true;
        type = Type.weapon;
        flavour = "A nice looking bow, with a conveniently endless quiver.";
        base.Init();
    }

    public override void Activate()
    {
        //MiniGameManager.MiniGameManage.MGArcheryAttack.StartMiniGame(P, Actor.actors[i], 20, 50, 0, "ATTACK");
    }

    public override void Attack(Actor target)
    {
        //MasMan.MGMan.MGArcheryAttack.StartMiniGame(Actor.player, target, actionCost, damageMin, damageMax, "ATTACK", mgFrequency, mgDuration);

        MasMan.MGMan.MGArcheryAttack.StartMiniGame
            (new MGDTO()
            {
                actorInitiated = Actor.player,
                actorTargeted = target,

                actionCost = mgDTO.actionCost,
                damageMin = mgDTO.damageMin,
                damageMax = mgDTO.damageMax,
                type = mgDTO.type,
                frequency = mgDTO.frequency,
                maxScore = mgDTO.maxScore,
                duration = mgDTO.duration,
            });
    }
}
