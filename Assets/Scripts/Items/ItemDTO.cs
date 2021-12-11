using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDTO : ScriptableObject
{
    public string label = "";
    public string flavour = " ";
    public string description = "";
    public int amount = 1;
    public int maxAmount = 1;
    public int value = 20;

    public MGDTO mgDTO = new MGDTO();

    /// <summary>
    /// minigame action cost is handled by mgdto, this is everything else
    /// </summary>
    public float actionCost;

    public Sprite sprite;

    public Type type;
    public enum Type
    {
        empty,
        consumable,
        weapon,
        misc
    };

    public virtual void Activate()
    {

    }

    public virtual void Attack(Actor target)
    {

    }

    public bool exists = false;

    public static ItemDTO CreateItem(string name)
    {
        ItemDTO itemToReturn = ScriptableObject.CreateInstance(name) as ItemDTO;
        itemToReturn.Init();

        return itemToReturn;
    }

    public static ItemDTO EmptyItem()
    {
        ItemDTO itemToReturn = ScriptableObject.CreateInstance("EmptyItem") as ItemDTO;
        itemToReturn.Init();

        return itemToReturn;
    }

    public static ItemDTO DeepCopy(ItemDTO _itemToCopyFrom)
    {
        ItemDTO _itemToCopyTo = Instantiate(CreateItem(_itemToCopyFrom.GetType().ToString()));

        _itemToCopyTo.label = _itemToCopyFrom.label;
        _itemToCopyTo.actionCost = _itemToCopyFrom.actionCost;
        _itemToCopyTo.flavour = _itemToCopyFrom.flavour;
        _itemToCopyTo.amount = _itemToCopyFrom.amount;
        _itemToCopyTo.description = _itemToCopyFrom.description;
        _itemToCopyTo.maxAmount = _itemToCopyFrom.maxAmount;
        _itemToCopyTo.mgDTO = _itemToCopyFrom.mgDTO;

        // insert copying here

        return _itemToCopyTo;
    }

    public virtual void Init()
    {
        sprite = Resources.Load<Sprite>("Items/" + this.GetType().ToString());
    }
}