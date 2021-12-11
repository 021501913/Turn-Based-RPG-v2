using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaItem : ItemDTO
{
    public override void Init()
    {
        label = "Banana";
        exists = true;
        type = Type.consumable;

        base.Init();
    }
}
