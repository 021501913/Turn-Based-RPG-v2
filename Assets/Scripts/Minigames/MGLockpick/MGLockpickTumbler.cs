using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGLockpickTumbler : MonoBehaviour
{
    public RectTransform pin;

    public Vector2 pinStartPosition;
    public Vector2 pinEndPosition;


    public bool started = false;
    public bool goingUp = false;

    public Vector3 velRef;

    public void StartPin()
    {
        started = true;
        goingUp = true;
    }

    private void Update()
    {

    }

}
