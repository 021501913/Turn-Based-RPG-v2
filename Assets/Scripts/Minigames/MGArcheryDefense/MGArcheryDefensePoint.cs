using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MGArcheryDefensePoint : MonoBehaviour
{
    public MiniGame miniGame;
    private RectTransform rect;
    public RectTransform shield;
    float timer = 0;

    public GameObject shadow;

    public float destroyDelay;

    private void Start()
    {
        rect = this.transform.GetComponent<RectTransform>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        shadow.transform.localScale = (Vector3.one * (1 - (timer / destroyDelay)) * 3);
    }

    public IEnumerator RemovePoint(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        string feedback = MGDTO.lesserFeedback[0];

        if (Vector2.Distance(rect.transform.position, shield.position) < 100)
        {
            miniGame.score += miniGame.mGDTO.maxScore / miniGame.mGDTO.frequency;
            feedback = MGDTO.lesserFeedback[4];
        }

        timer = 0;

        GameObject popupTextInstance = MasMan.PreMan.SpawnPrefab((MasMan.PreMan.popupText));

        RectTransform popupRect = popupTextInstance.GetComponent<RectTransform>();
        popupRect.transform.SetParent(MasMan.MGMan.transform);
        popupRect.transform.GetComponentInChildren<TextMeshProUGUI>().text = feedback;
        popupRect.transform.position = shield.transform.position;

        Destroy(this.gameObject);
    }

    public void OnEnable()
    {
        miniGame.targets++;
        miniGame.targetGOs.Add(this.gameObject);
    }

    public void OnDisable()
    {
        miniGame.targets--;
        miniGame.targetGOs.Remove(this.gameObject);
    }
}
