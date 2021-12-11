using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGArcheryDefense : MiniGame
{
    public GameObject targetPrefab;
    public MGArcheryShield shield;

    //public int targetAmount = 5;
    //public float duration = 2;

    public override void StartMiniGame(MGDTO _mgdto)
    {
        base.StartMiniGame(_mgdto);

        shield.rect.transform.localPosition = new Vector3(0, 150, 0);
    }

    public override void EndMiniGame()
    {
        base.EndMiniGame();

        for (int i = 0; i < targetGOs.Count; i++)
            Destroy(targetGOs[i]);

        shield.Release();
    }

    public override IEnumerator StartMiniGameCor()
    {
        float spawnDelay = mGDTO.duration / mGDTO.frequency;

        yield return new WaitForSeconds(mGDTO.startDelay);

        for (int i = 0; i < mGDTO.frequency; i++)
        {
            AddTarget();
            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitForSeconds(mGDTO.endDelay);

        EndMiniGame();
    }

    public void AddTarget()
    {
        GameObject targetPrefabInstance = MasMan.PreMan.SpawnPrefab(targetPrefab);

        float randomRange = 200;
        RectTransform rect = targetPrefabInstance.transform.GetComponent<RectTransform>();
        rect.SetParent(this.transform);
        rect.localPosition = new Vector2(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));
        Destroy(targetPrefabInstance, 10);

        MGArcheryDefensePoint target = targetPrefabInstance.GetComponent<MGArcheryDefensePoint>();
        target.destroyDelay = (mGDTO.duration / mGDTO.frequency) - 0.1f;
        StartCoroutine(target.RemovePoint((mGDTO.duration / mGDTO.frequency) - 0.1f));

        //return targetPrefabInstance;
    }
}
