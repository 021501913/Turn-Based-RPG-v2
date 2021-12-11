using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NesScripts.Controls.PathFind;

public class GridManager : MonoBehaviour
{
    public GameObject gridPiecePrefab;

    // create the tiles map
    [SerializeField]
    public float[,] tilesmap = new float[100, 100];
    // set values here....
    // every float in the array represent the cost of passing the tile at that position.
    // use 0.0f for blocking tiles.

    Point point = new Point(0, 0);
    public List<GridPiece> gridPieces = new List<GridPiece>();
    [SerializeField]
    public NesScripts.Controls.PathFind.Grid grid;

    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    public Texture2D noiseTex;
    public Color[] pix;
    public Renderer rend;
    void Start()
    {

    }

    private void Awake()
    {
        /*
        // Set up the texture and a Color array to hold pixels during processing.
        pixWidth = tilesmap.GetLength(0);
        pixHeight = tilesmap.GetLength(1);
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        noiseTex.filterMode = FilterMode.Point;
        rend.material.mainTexture = noiseTex;

        CalcNoise();
        */
    }

    public void CreateMap()
    {
        // create map
        int i = 0;
        for (int x = 0; x < tilesmap.GetLength(0); x++)
        {
            for (int y = 0; y < tilesmap.GetLength(1); y++)
            {
                gridPieces.Add(new GridPiece());
                tilesmap[x, y] = 0;

                GameObject gridPiecePrefabInstance = Instantiate(gridPiecePrefab, this.transform, false);
                gridPiecePrefabInstance.transform.position = new Vector3(x * 5, -0, y * 5);
                GridPiece gPiece = gridPiecePrefabInstance.GetComponent<GridPiece>();

                gridPieces[i] = gPiece;
                gridPieces[i].MakeClear();

                gPiece.pos.x = x;
                gPiece.pos.y = y;

                gridPiecePrefabInstance.name = x.ToString() + " | " + y.ToString();
                gridPiecePrefabInstance.SetActive(true);

                i++;
            }
        }

        grid = new NesScripts.Controls.PathFind.Grid(tilesmap);
        grid.UpdateGrid(tilesmap);
    }

    public void ClearMap()
    {
        gridPieces.Clear();

        for (int i = 0; i < this.transform.childCount; i++)
            Destroy(this.transform.GetChild(i).gameObject);
    }

    public int GetRoomInt(Vector2Int pos, List<Vector2Int> roomPositions)
    {
        for (int i = 0; i < roomPositions.Count; i++)
            if (pos == roomPositions[i])
                return i;

        return -1;
    }

    public GridPiece FindGridPiece(int x, int y)
    {
        for (int i = 0; i < gridPieces.Count; i++)
            if (gridPieces[i].pos.x == x && gridPieces[i].pos.y == y)
                return gridPieces[i];

        Debug.Log("COULD NOT FIND GRIDPIECE");
        return null;
    }

    void CalcNoise()
    {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    private void Update()
    {
        //CalcNoise();

        if (Input.GetMouseButtonDown(1))
        {
            //return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GridPiece gPiece = hit.collider.GetComponent<GridPiece>();
                Debug.Log(hit.collider.gameObject.name);
                if (gPiece != null)
                {
                    if (gPiece.passable)
                    {
                        tilesmap[gPiece.pos.x, gPiece.pos.y] = 0.0f;
                        gPiece.MakeImpassable();
                    }
                    else
                    {
                        tilesmap[gPiece.pos.x, gPiece.pos.y] = 1;
                        gPiece.MakePassable();
                    }
                }

                grid.UpdateGrid(tilesmap);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GridPiece gPiece = hit.collider.GetComponent<GridPiece>();
                Debug.Log(hit.collider.gameObject.name);
                if (gPiece != null)
                {

                    List<NesScripts.Controls.PathFind.Point> path =
                        NesScripts.Controls.PathFind.Pathfinding.FindPath(grid,
                        point,
                        new NesScripts.Controls.PathFind.Point(gPiece.pos.x, gPiece.pos.y),
                        NesScripts.Controls.PathFind.Pathfinding.DistanceType.Manhattan);

                    point = new NesScripts.Controls.PathFind.Point(gPiece.pos.x, gPiece.pos.y);

                    for (int i = 0; i < path.Count; i++)
                    {
                        for (int j = 0; j < gridPieces.Count; j++)
                        {
                            if (gridPieces[j].pos.x == path[i].x && gridPieces[j].pos.y == path[i].y)
                            {
                                gridPieces[j].MakeImpassable();
                            }
                        }
                    }

                    grid.UpdateGrid(tilesmap);

                }
            }
        }
    }

}