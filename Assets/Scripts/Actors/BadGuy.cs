using System.Collections;
using System.Collections.Generic;
using NesScripts.Controls.PathFind;
using UnityEngine;
using TMPro;

public class BadGuy : Actor
{
    public Vector2 forward = new Vector2(0, 1);
    public Vector2 right = new Vector2(1, 0);
    public Vector2 left = new Vector2(-1, 0);
    public Vector2 back = new Vector2(0, -1);




    private void Start()
    {
        MoveTo(new Point(startingPosition.x, startingPosition.y));
        SetRandomActionPoints();
    }

    private void SetRandomActionPoints()
    {
        int id = 0;
        for (int i = 0; i < actors.Count; i++)
            if (actors[i] == this)
                id = i;

        actionPoints = 0 + id;

        if (actionPoints > 100)
            actionPoints -= 100;


    }


    public override void Update()
    {
        base.Update();

        if (isTurn && hasInput)
        {
            List<Point> path =
                   Pathfinding.FindPath(MasMan.GridMan.grid,
                   position,
                   Actor.player.position,
                   Pathfinding.DistanceType.Manhattan);

            if (path.Count > 1)
            {
                MoveTo(path[0]);
                CompleteTurn(20, 0);
                anim.Play("move", -1, 0.0f);

            }
            else if (path.Count == 1)
            {
                // attack the player
                //Actor.player.TakeDamage(20);

                if (position == new Point(Actor.player.position.x + Mathf.RoundToInt(-Actor.player.forward.x), Actor.player.position.y + Mathf.RoundToInt(Actor.player.forward.y)))
                {
                    MasMan.MGMan.MGArcheryDefense.StartMiniGame
                        (new MGDTO()
                        {
                            actorInitiated = this,
                            actorTargeted = Actor.player,
                            damageMin = 15,
                            damageMax = 30,
                            type = "DEFENSE",
                            frequency = 3,
                            maxScore = 15,
                            duration = 2,
                            actionCost = 30,
                        });

                    //MasMan.MGMan.MGArcheryDefense.StartMiniGame(this, Actor.player, 50, 0, 50, "DEFENSE", 3, 5);
                }
                else if (position == new Point(Actor.player.position.x + Mathf.RoundToInt(-Actor.player.left.x), Actor.player.position.y + Mathf.RoundToInt(Actor.player.left.y)))
                {
                    Actor.player.anim.Play("TakeDamageLeft", -1, 0.0f);
                    anim.Play("attack", -1, 0.0f);
                    Actor.player.TakeDamage(20);
                    CompleteTurn(50, 0.1f);
                }
                else if (position == new Point(Actor.player.position.x + Mathf.RoundToInt(-Actor.player.right.x), Actor.player.position.y + Mathf.RoundToInt(Actor.player.right.y)))
                {
                    Actor.player.anim.Play("TakeDamageRight", -1, 0.0f);
                    anim.Play("attack", -1, 0.0f);
                    Actor.player.TakeDamage(20);
                    CompleteTurn(50, 0.1f);
                }
                else if (position == new Point(Actor.player.position.x + Mathf.RoundToInt(-Actor.player.back.x), Actor.player.position.y + Mathf.RoundToInt(Actor.player.back.y)))
                {
                    Actor.player.anim.Play("TakeDamageBehind", -1, 0.0f);
                    Actor.player.TakeDamage(20);
                    anim.Play("attack", -1, 0.0f);
                    CompleteTurn(50, 0.1f);
                }
                else
                {
                    CompleteTurn(50, 0);
                }

                hasInput = false;
            }
            else
            {
                Patrol();
            }
            //MoveInDirection(newPos);


            //CompleteTurn(20);
        }
    }

    public override void MoveTo(Point pos)
    {
        MasMan.GridMan.tilesmap[pos.x, pos.y] = 0;
        MasMan.GridMan.tilesmap[position.x, position.y] = 1; // could cause issues if enemy transitions through levels

        MasMan.GridMan.grid.UpdateGrid(MasMan.GridMan.tilesmap);

        base.MoveTo(pos);
    }

    public override void TakeDamage(float _damage)
    {
        anim.Play("takedamage", -1, 0.0f);

        GameObject damageTextInstance = Instantiate(MasMan.PreMan.enemyDamageText, null);
        damageTextInstance.SetActive(true);
        damageTextInstance.transform.position = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        damageTextInstance.transform.GetComponentInChildren<TextMeshPro>().text = _damage.ToString();
        Destroy(damageTextInstance, 5);

        base.TakeDamage(_damage);

        if (healthPoints <= 0 && !isPlayer)
        {
            for (int i = 0; i < MasMan.GridMan.gridPieces.Count; i++)
            {
                if (MasMan.GridMan.gridPieces[i].pos == position) // target position
                {
                    MasMan.GridMan.tilesmap[MasMan.GridMan.gridPieces[i].pos.x, MasMan.GridMan.gridPieces[i].pos.y] = 1;
                    MasMan.GridMan.gridPieces[i].MakePassable();
                }
            }

            MasMan.GridMan.grid.UpdateGrid(MasMan.GridMan.tilesmap);

            Destroy(this.gameObject);
        }
    }


    public void Patrol()
    {
        float randomNumber = Random.Range(0f, 1f);

        if (randomNumber < 0.2f)
        {
            Point newPos = new Point(position.x + Mathf.RoundToInt(-forward.x), position.y + Mathf.RoundToInt(forward.y));

            MoveInDirection(newPos);
        }
        else if (randomNumber < 0.4f)
        {
            Point newPos = new Point(position.x + Mathf.RoundToInt(-back.x), position.y + Mathf.RoundToInt(back.y));

            MoveInDirection(newPos);
        }
        else if (randomNumber < 0.6f)
        {
            Point newPos = new Point(position.x + Mathf.RoundToInt(-left.x), position.y + Mathf.RoundToInt(left.y));

            MoveInDirection(newPos);
        }
        else if (randomNumber < 0.8f)
        {
            Point newPos = new Point(position.x + Mathf.RoundToInt(-right.x), position.y + Mathf.RoundToInt(right.y));

            MoveInDirection(newPos);
        }
        else
        {
            CompleteTurn(20, 0);
        }
    }
}
