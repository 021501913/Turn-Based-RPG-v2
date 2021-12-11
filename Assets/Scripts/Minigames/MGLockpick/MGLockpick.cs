using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGLockpick : MiniGame
{
    public List<MGLockpickTumbler> tumblers = new List<MGLockpickTumbler>();

    public int currentTumbler = 0;

    public override void StartMiniGame(MGDTO _mgdto)
    {
        base.StartMiniGame(_mgdto);

        timer = 0.25f;

        for (int i = 0; i < tumblers.Count; i++)
        {
            if (i > mGDTO.frequency - 1)
            {
                tumblers[i].started = false;
                //currentTumbler++;
                //timer = 0.25f;
                tumblers[i].pin.localPosition = tumblers[i].pinEndPosition;
                tumblers[i].velRef = Vector3.zero;
            }
        }
    }

    public override void EndMiniGame()
    {
        for (int i = 0; i < tumblers.Count; i++)
        {
            tumblers[i].pin.localPosition = tumblers[i].pinStartPosition;
            tumblers[i].velRef = Vector3.zero;
        }

        currentTumbler = 0;

        base.EndMiniGame();
    }

    float timer = 0;

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer > 0)
            return;


        if (currentTumbler == mGDTO.frequency)
        {
            score = 1;
            Loot lootTarget = (Loot)mGDTO.actorTargeted;
            lootTarget.isLocked = false;
            EndMiniGame();
        }

        for (int i = 0; i < mGDTO.frequency; i++)
        {
            if (i == currentTumbler)
            {
                if (tumblers[i].started)
                {
                    if (tumblers[i].goingUp)
                    {
                        tumblers[i].pin.localPosition = Vector3.SmoothDamp(tumblers[i].pin.localPosition, tumblers[i].pinEndPosition, ref tumblers[i].velRef, 0.1f);

                        if (Vector3.Distance(tumblers[i].pin.localPosition, tumblers[i].pinEndPosition) < 0.05f)
                        {
                            tumblers[i].goingUp = false;
                        }
                    }

                }

                if (!tumblers[i].goingUp)
                {
                    tumblers[i].pin.localPosition = Vector3.SmoothDamp(tumblers[i].pin.localPosition, tumblers[i].pinStartPosition, ref tumblers[i].velRef, 0.5f);

                    if (Vector3.Distance(tumblers[i].pin.localPosition, tumblers[i].pinStartPosition) < 50f)
                    {
                        tumblers[i].started = false;
                    }

                    if (Vector3.Distance(tumblers[i].pin.localPosition, tumblers[i].pinStartPosition) < 0.05f)
                    {
                        //goingUp = true;
                    }
                }

                if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
                {
                    if (!tumblers[i].started)
                    {
                        tumblers[i].StartPin();
                    }
                    else
                    {
                        if (Vector3.Distance(tumblers[i].pin.localPosition, tumblers[i].pinEndPosition) < 2f)
                        {
                            tumblers[i].started = false;
                            currentTumbler++;
                            timer = 0.25f;
                            return;
                        }
                        else
                        {
                            score = 0;
                            EndMiniGame();
                            return;
                        }
                    }
                }
            }
        }
    }
}
