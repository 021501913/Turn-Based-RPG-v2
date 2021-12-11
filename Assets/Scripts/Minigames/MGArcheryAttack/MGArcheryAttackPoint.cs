using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MGArcheryAttackPoint : MonoBehaviour
{
    public MiniGame miniGame;
    public bool hovering = false;
    int score = 0;

    public GameObject parentGO;
    public MGArcheryAttackPoint parentTarget;

    bool triggered = false;

    float timer = 0;

    private void Update()
    {

    }

    public IEnumerator RemovePoint(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        string feedback = MGDTO.lesserFeedback[0];

        timer = 0;

        GameObject popupTextInstance = MasMan.PreMan.SpawnPrefab((MasMan.PreMan.popupText));

        RectTransform popupRect = popupTextInstance.GetComponent<RectTransform>();
        popupRect.transform.SetParent(MasMan.MGMan.transform);
        popupRect.transform.GetComponentInChildren<TextMeshProUGUI>().text = feedback;
        popupRect.transform.position = Input.mousePosition;

        Destroy(this.gameObject);
    }

    public void HitTarget(int score)
    {
        if (parentTarget.triggered)
            return;

        string feedback = MGDTO.lesserFeedback[score];

        GameObject popupTextInstance = MasMan.PreMan.SpawnPrefab(MasMan.PreMan.popupText);

        RectTransform rect = popupTextInstance.GetComponent<RectTransform>();
        rect.transform.SetParent(MasMan.MGMan.transform);
        rect.transform.localPosition = parentGO.transform.localPosition;
        rect.transform.GetComponentInChildren<TextMeshProUGUI>().text = feedback;

        Destroy(popupTextInstance, 5);

        miniGame.score += score;

        parentTarget.triggered = true;

        Destroy(parentGO);
    }

    public void OnHover()
    {
        hovering = true;
    }

    public void OffHover()
    {
        hovering = false;
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
