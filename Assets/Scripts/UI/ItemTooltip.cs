using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    private static ItemTooltip _itemTooltip;
    public RectTransform rect;
    public static ItemTooltip ItemToolti
    {
        get
        {
            if (_itemTooltip == null)
            {
                _itemTooltip = GameObject.FindObjectOfType<ItemTooltip>();
            }

            return _itemTooltip;
        }
    }
    public TextMeshProUGUI label;
    public TextMeshProUGUI flavour;
    public TextMeshProUGUI content;
    public TextMeshProUGUI uses;
    public TextMeshProUGUI type;
    public TextMeshProUGUI stamina;
    public TextMeshProUGUI value;


    void Awake()
    {
        rect = this.GetComponent<RectTransform>();
        ItemToolti.gameObject.SetActive(false);
        //HideTooltip();
    }

    void Update()
    {
        this.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y + rect.rect.height + 50);
    }

    public void ApplyTooltip(ItemDTO item)
    {
        label.text = item.label;
        flavour.text = item.flavour;
        if (item.type == ItemDTO.Type.weapon)
            content.text = "Deals " + item.mgDTO.damageMin + " to " + item.mgDTO.damageMax + " damage" + "\n" + item.description;
        else
            content.text = item.description;

        uses.text = "[" + item.amount + "/" + item.maxAmount + "]";

        type.text = item.type.ToString();

        if (item.actionCost > 0)
            stamina.text = item.actionCost.ToString() + " STA";
        else
            stamina.text = "";

        value.text = "SELLS FOR " + item.value;


    }

    public void ShowTooltip()
    {
        this.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        this.gameObject.SetActive(false);
    }
}
