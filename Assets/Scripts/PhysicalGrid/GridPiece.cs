using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPiece : MonoBehaviour
{
    public NesScripts.Controls.PathFind.Point pos = new NesScripts.Controls.PathFind.Point(0, 0);

    public GameObject blue;
    public GameObject black;
    public GameObject clear;

    public bool passable = true;

    public void MakePassable()
    {
        black.SetActive(false);
        blue.SetActive(true);
        clear.SetActive(false);
        passable = true;
    }

    public void MakeImpassable()
    {
        black.SetActive(true);
        blue.SetActive(false);
        clear.SetActive(false);
        passable = false;
    }

    public void MakeClear()
    {
        black.SetActive(false);
        blue.SetActive(false);
        clear.SetActive(true);
        passable = false;
    }
}
