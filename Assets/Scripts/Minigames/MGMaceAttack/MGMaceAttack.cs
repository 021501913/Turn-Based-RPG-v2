using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGMaceAttack : MiniGame
{
    public GameObject clubPrefab;

    public override IEnumerator StartMiniGameCor()
    {
        float spawnDelay = mGDTO.duration / mGDTO.frequency;
        mGDTO.maxScore = mGDTO.frequency * 4;

        yield return new WaitForSeconds(mGDTO.startDelay);

        for (int i = 0; i < mGDTO.frequency; i++)
        {
            SpawnClub();
            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitForSeconds(mGDTO.endDelay);

        EndMiniGame();
    }

    public void SpawnClub()
    {
        GameObject clubGOInstance = Instantiate(clubPrefab, this.transform);
        clubGOInstance.SetActive(true);

        MGMaceAttackPoint attackPoint = clubGOInstance.GetComponent<MGMaceAttackPoint>();
        attackPoint.Reset();

        attackPoint.delay = mGDTO.duration / mGDTO.frequency;

    }
}
