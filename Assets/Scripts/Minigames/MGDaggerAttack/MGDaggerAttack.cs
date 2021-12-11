using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGDaggerAttack : MiniGame
{
    public List<MGDaggerAttackPoint> attackPoints = new List<MGDaggerAttackPoint>();

    public override void StartMiniGame(MGDTO _mgdto)
    {
        base.StartMiniGame(_mgdto);

        timer = 0;

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);

        attackPoints = new List<MGDaggerAttackPoint>();
        attackPoints.AddRange(this.GetComponentsInChildren<MGDaggerAttackPoint>());

        mGDTO.maxScore = attackPoints.Count;

        for (int i = 0; i < attackPoints.Count; i++)
            attackPoints[i].Reveal();
    }

    public override void EndMiniGame()
    {

        base.EndMiniGame();


    }

    float timer = 0;

    private void Update()
    {
        if (started)
        {
            timer += Time.deltaTime;

            if (timer > 1.5f || attackPoints.Count == 0)
            {
                EndMiniGame();
            }

            for (int i = 0; i < attackPoints.Count; i++)
            {
                if (attackPoints[i].hovering && i == 0)
                {
                    if (Input.GetMouseButton(0))
                    {
                        score++;
                        attackPoints[i].Hide();

                        attackPoints.RemoveAt(0);
                    }
                }
            }
        }
    }
}
