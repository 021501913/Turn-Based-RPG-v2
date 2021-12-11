using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGArcheryAttack : MiniGame
{
    public GameObject targetPrefab;

    public override void EndMiniGame()
    {
        for (int i = 0; i < targetGOs.Count; i++)
        {
            Destroy(targetGOs[i]);
        }

        base.EndMiniGame();
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
        targetPrefabInstance.SetActive(true);

        float randomRange = 200;
        RectTransform rect = targetPrefabInstance.transform.GetComponent<RectTransform>();
        rect.SetParent(this.transform);
        rect.localPosition = new Vector2(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));
        MGArcheryAttackPoint target = targetPrefabInstance.GetComponent<MGArcheryAttackPoint>();

        target.StartCoroutine(target.RemovePoint((mGDTO.duration / mGDTO.frequency) - 0.1f));
    }
}
