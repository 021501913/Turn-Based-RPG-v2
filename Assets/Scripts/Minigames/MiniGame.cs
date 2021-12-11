using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGame : MonoBehaviour
{
    public bool started = false;
    public bool ended = false;

    public int targets = 0;
    public List<GameObject> targetGOs = new List<GameObject>();

    [SerializeField]
    public MGDTO mGDTO = new MGDTO();

    public int score;
    public int perfectScore = 15;

    // -- to be deleted

    public static readonly string[] smallFeedback = new string[]
{
        "MISSED",
        "HIT",
        "DECENT",
        "NICE",
        "GREAT",
        "PERFECT",
};

    public static readonly string[] largeFeedback = new string[]
{
        "MEDIOCRE",
        "DECENT",
        "NICE",
        "GREAT",
        "FLAWLESS",
};

    // --

    public virtual void StartMiniGame(MGDTO _mgdto)
    {
        mGDTO = _mgdto;

        mGDTO.actorInitiated.hasInput = false;

        this.transform.gameObject.SetActive(true);

        started = true;
        ended = false;

        if (MasMan.InventoryManager.loot.open)
            MasMan.InventoryManager.loot.CloseLoot();

        StartCoroutine(StartMiniGameCor());
    }

    public virtual IEnumerator StartMiniGameCor()
    {
        yield return new WaitForEndOfFrame();
    }

    public virtual void EndMiniGame()
    {
        string feedback = MGDTO.greaterFeedback[0];

        if (score > mGDTO.maxScore)
            score = mGDTO.maxScore;

        float compressedScore = (float)score / (float)mGDTO.maxScore;

        for (int i = 0; i < MGDTO.greaterFeedback.Length; i++)
            if (compressedScore >= (1 / ((float)MGDTO.greaterFeedback.Length - 1)) * i)
                feedback = MGDTO.greaterFeedback[i];

        GameObject popupTextInstance = MasMan.PreMan.SpawnPrefab(MasMan.PreMan.popupTextLarge);

        RectTransform rect = popupTextInstance.GetComponent<RectTransform>();
        rect.transform.SetParent(MasMan.MGMan.transform);
        rect.transform.GetComponentInChildren<TextMeshProUGUI>().text = feedback + " " + mGDTO.type;
        rect.transform.localPosition = this.transform.localPosition;

        Destroy(popupTextInstance, 5);

        started = false;
        ended = true;

        float damageToTake = 0;

        if (mGDTO.actorInitiated is Player)
            damageToTake = mGDTO.damageMin;
        else if (mGDTO.actorInitiated is BadGuy)
            damageToTake = mGDTO.damageMax;

        float reducedDamage = mGDTO.damageMax - mGDTO.damageMin;
        reducedDamage *= (float)score / (float)mGDTO.maxScore;

        if (mGDTO.actorInitiated is BadGuy)
            damageToTake -= reducedDamage;
        else if (mGDTO.actorInitiated is Player)
            damageToTake += reducedDamage;

        if (mGDTO.actorTargeted.isPlayer && damageToTake > 5f)
        {
            Actor.player.anim.Play("TakeDamageForward", -1, 0.0f);
        }
        else if (mGDTO.actorTargeted is BadGuy && damageToTake > 5)
        {
            mGDTO.actorInitiated.anim.Play("attack", -1, 0.0f);

        }

        score = 0;

        if (mGDTO.actorInitiated.isPlayer && mGDTO.actorTargeted is BadGuy)
            Actor.player.anim.Play("Attack", -1, 0.0f);


        mGDTO.actorTargeted.TakeDamage(Mathf.RoundToInt(damageToTake));
        mGDTO.actorInitiated.CompleteTurn(mGDTO.actionCost, 0.3f);

        this.transform.gameObject.SetActive(false);
    }
}
