using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyItem : ItemDTO
{
    public override void Init()
    {
        label = "EmptyItem";
        exists = false;

        base.Init();
    }
}
