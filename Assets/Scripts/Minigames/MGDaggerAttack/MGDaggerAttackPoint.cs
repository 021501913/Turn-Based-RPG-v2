using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MGDaggerAttackPoint : MonoBehaviour
{
    public Image image;
    public Color highlightedColor = Color.green;
    public Color normalColor = Color.gray;

    public MGDaggerAttack miniGame;
    public bool hovering = false;

    public bool highlighted = false;

    private RectTransform rect;

    private void Start()
    {
        rect = this.GetComponent<RectTransform>();
    }

    public void Reveal()
    {
        highlighted = false;
        this.gameObject.SetActive(true);
        image.color = normalColor;

    }

    public void Hide()
    {
        highlighted = false;
        this.gameObject.SetActive(false);
        image.color = normalColor;
        rect.localScale = Vector3.one * 0f;
        OffHover();
    }

    public void Highlight()
    {
        highlighted = true;
        this.gameObject.SetActive(true);
        image.color = highlightedColor;
    }

    private void Update()
    {

        if (highlighted)
        {
            rect.localScale = Vector3.one * 0.5f;
        }
        else
        {



            float distanceCap = 175;
            float distanceMultiplier = (distanceCap - Vector3.Distance(this.transform.position, miniGame.attackPoints[0].transform.position)) / distanceCap;
            if (distanceMultiplier < 0)
                distanceMultiplier = 0;

            //distanceMultiplier = Mathf.Clamp(distanceMultiplier, 0, 0.4f);

            rect.localScale = (Vector3.one * 0.5f) * distanceMultiplier;

            //Debug.Log(distanceMultiplier.ToString());
        }
    }






    public void OnHover()
    {
        hovering = true;
    }

    public void OffHover()
    {
        hovering = false;
    }
}
