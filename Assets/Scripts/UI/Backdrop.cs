using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backdrop : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public bool hovered = false;

    private void Update()
    {
        if (hovered)
        {
            if (Input.GetMouseButtonDown(0))
            {
                inventoryManager.MouseDownOnBackground();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                inventoryManager.MouseUpOnBackground();
            }
        }
    }

    public void OnHover()
    {
        //Debug.Log("hover on");
        hovered = true;
    }

    public void OffHover()
    {
        //Debug.Log("hover off");
        hovered = false;
    }
}
