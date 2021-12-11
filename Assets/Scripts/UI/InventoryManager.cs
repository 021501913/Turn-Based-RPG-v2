using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Controls.PathFind;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{


    public Image draggedImage;

    public LootInventory loot;

    public Slot pickedUpItem;

    private void Awake()
    {

    }

    private void Update()
    {
        if (pickedUpItem != null)
        {
            draggedImage.transform.position = Input.mousePosition;
        }
    }

    public void PickupItem(Slot slot)
    {
        pickedUpItem = slot;
        draggedImage.enabled = true;
        draggedImage.sprite = pickedUpItem.item.sprite;
    }

    public void PlaceItem(Slot _slotToPlaceIn)
    {
        _slotToPlaceIn.item = pickedUpItem.item;
        pickedUpItem.item = ItemDTO.EmptyItem();
        ClearDraggedItem();
    }

    public void SwapItems(Slot _SwapTo)
    {
        ItemDTO deepCopy = ItemDTO.DeepCopy(_SwapTo.item);

        _SwapTo.item = pickedUpItem.item;
        pickedUpItem.item = deepCopy;

        ClearDraggedItem();
    }

    public void ClearDraggedItem()
    {
        draggedImage.enabled = false;
        pickedUpItem = null;
    }

    public void MouseDownOnBackground()
    {
        if (Actor.player.hasInput && Actor.player.isTurn)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Actor hitActor = hit.collider.GetComponent<Actor>();

                if (hitActor != null)
                {
                    Point newPos = new Point(Actor.player.position.x, Actor.player.position.y);
                    if (hitActor.position == newPos && hitActor is Loot)
                    {
                        Loot lootActor = (Loot)hitActor;

                        if (!lootActor.isLocked)
                        {
                            Debug.Log("HIT: " + hit.collider.GetComponent<Actor>().name);
                            lootActor.Interacted();
                        }

                    }
                }
            }
        }
    }


    public void MouseUpOnBackground()
    {
        if (pickedUpItem != null)
        {
            if (Actor.player.hasInput && Actor.player.isTurn && !loot.open)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Actor hitActor = hit.collider.GetComponent<Actor>();

                    if (hitActor != null)
                    {
                        Point newPos = new Point(Actor.player.position.x + Mathf.RoundToInt(-Actor.player.forward.x), Actor.player.position.y + Mathf.RoundToInt(Actor.player.forward.y));

                        if (hitActor.position == newPos && hitActor is BadGuy && pickedUpItem.item.type == ItemDTO.Type.weapon)
                        {
                            pickedUpItem.item.Attack(hitActor);
                        }
                        else if (hitActor.position == Actor.player.position && hitActor is Loot && pickedUpItem.item is LockpickItem)
                        {
                            Loot lootActor = (Loot)hitActor;
                            Debug.Log("HIT: " + hit.collider.GetComponent<Actor>().name);

                            if (lootActor.isLocked)
                            {
                                pickedUpItem.item.Attack(lootActor);
                            }
                        }
                    }
                }
            }

            ClearDraggedItem();
        }
    }
}
