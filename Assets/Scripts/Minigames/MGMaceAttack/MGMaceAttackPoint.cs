using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MGMaceAttackPoint : MonoBehaviour
{
    public MiniGame miniGame;

    float progress = 0;
    public RectTransform rect;
    public RectTransform visualRect;

    public float delay;

    private void Update()
    {
        progress += Time.deltaTime / delay;

        #region Cosmetic
        rect.transform.localScale = Vector3.one * (progress * 1.2f);

        visualRect.localScale = Vector3.one * (1.2f - (progress * 1.2f));
        #endregion

        if (Input.GetKeyDown(KeyCode.Space))
        {
            string scoreText = MGDTO.lesserFeedback[0];
            float[] progressScores = new float[] { 0.75f, 0.80f, 0.85f, 0.9f, 1};
            bool con = false;

            for (int i = 0; i < progressScores.Length; i++)
            {
                if (progress <= progressScores[i] && !con)
                {
                    miniGame.score += i;
                    scoreText = MGDTO.lesserFeedback[i];
                    con = true;
                }
            }

            GameObject popupTextInstance = MasMan.PreMan.SpawnPrefab(MasMan.PreMan.popupText);
            RectTransform popupRect = popupTextInstance.GetComponent<RectTransform>();
            popupRect.transform.SetParent(MasMan.MGMan.transform);
            popupRect.transform.GetComponentInChildren<TextMeshProUGUI>().text = scoreText;
            popupRect.transform.position = rect.position;

            Destroy(this.gameObject);
            return;
        }

        if (progress > 1)
        {
            GameObject popupTextInstance = MasMan.PreMan.SpawnPrefab(MasMan.PreMan.popupText);
            RectTransform popupRect = popupTextInstance.GetComponent<RectTransform>();
            popupRect.transform.SetParent(MasMan.MGMan.transform);
            popupRect.transform.GetComponentInChildren<TextMeshProUGUI>().text = MiniGame.smallFeedback[0];
            popupRect.transform.position = rect.position;

            Destroy(this.gameObject);
            return;
        }
    }



    public void Reset()
    {
        progress = 0;
    }
}
