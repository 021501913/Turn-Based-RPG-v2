using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGArcheryShield : MonoBehaviour
{
    public bool pickedUp = false;
    public RectTransform rect;

    private void Start()
    {
        rect = this.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (pickedUp)
            transform.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void Pickup()
    {
        pickedUp = true;
    }

    public void Release()
    {
        pickedUp = false;
    }
}
