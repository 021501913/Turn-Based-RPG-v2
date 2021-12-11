using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Controls.PathFind;

public class StairsUp : Actor
{
    public override void OnEnable()
    {
        stairsUp = this;
        base.OnEnable();
    }

    public override void MoveTo(Point pos)
    {
        MasMan.GridMan.tilesmap[pos.x, pos.y] = 0;
        //MasMan.GridMan.tilesmap[position.x, position.y] = 1;

        MasMan.GridMan.grid.UpdateGrid(MasMan.GridMan.tilesmap);

        base.MoveTo(pos);
    }

    public override void Update()
    {
        base.Update();

        if (isTurn && hasInput)
        {
            CompleteTurn(100,0);
        }
    }
}
