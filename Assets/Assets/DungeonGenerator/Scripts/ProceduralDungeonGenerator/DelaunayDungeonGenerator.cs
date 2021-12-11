using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Delaunay;
using Delaunay.Geo;

namespace DI.DungeonGenerator
{
    //this implementation need https://github.com/jceipek/Unity-delaunay. It is MIT license, project already imported.
    public class DelaunayDungeonGenerator : MonoBehaviour
    {


        public DungeonRoomsGenerator dg { get { if (m_dg == null) m_dg = GetComponent<DungeonRoomsGenerator>(); return m_dg; } }
        private DungeonRoomsGenerator m_dg;

        [SerializeField, Range(0f, 1f)] private float mainRoomSize = 0.5f;
        [SerializeField, Range(0f, .1f)] private float keepConnection = 0.05f;

        [HideInInspector] public List<Room> mainRooms = new List<Room>();

        //Delaunay
        Delaunay.Voronoi v;
        private List<LineSegment> m_spanningTree;
        private List<LineSegment> m_delaunayTriangulation;
#if DEBUG_MODE
        public bool drawTriangulation = true;
        public bool drawSpanningTree = true;
#endif

        void Start()
        {
            MakeMap();
        }

        public void MakeMap()
        {
            #region Clear Bad Guys
            List<BadGuy> badGuysToRemove = new List<BadGuy>();
            for (int i = 0; i < Actor.actors.Count; i++)
                if (Actor.actors[i] is BadGuy)
                    badGuysToRemove.Add((BadGuy)Actor.actors[i]);

            for (int i = 0; i < badGuysToRemove.Count; i++)
                Destroy(badGuysToRemove[i].gameObject);

            #endregion

            DoGeneration();

            #region Shift Delaunay Dungeon Generation above 0
            float lowestX = 500;
            float lowestY = 500;

            // roomPossibleConnections

            for (int i = 0; i < mainRooms.Count; i++)
            {
                if (mainRooms[i].position.x < lowestX)
                    lowestX = mainRooms[i].position.x;

                if (mainRooms[i].position.y < lowestY)
                    lowestY = mainRooms[i].position.y;
            }

            for (int i = 0; i < mainRooms.Count; i++)
            {
                for (int j = 0; j < mainRooms[i].doorPositions.Count; j++)
                {
                    if (mainRooms[i].doorPositions[j].corridor.x < lowestX)
                        lowestX = mainRooms[i].doorPositions[j].corridor.x;

                    if (mainRooms[i].doorPositions[j].corridor.y < lowestY)
                        lowestY = mainRooms[i].doorPositions[j].corridor.y;
                }
            }

            lowestX -= 3;
            lowestY -= 3;

            for (int i = 0; i < mainRooms.Count; i++)
            {
                mainRooms[i].rect = new Rect(mainRooms[i].rect.position.x - lowestX, mainRooms[i].rect.position.y - lowestY, mainRooms[i].rect.width, mainRooms[i].rect.height);
            }

            for (int i = 0; i < mainRooms.Count; i++)
            {
                for (int j = 0; j < mainRooms[i].doorPositions.Count; j++)
                {
                    if (mainRooms[i].doorPositions[j].corridor.width < 0)
                    {
                        mainRooms[i].doorPositions[j].corridor =
                        new Rect(
                        mainRooms[i].doorPositions[j].corridor.x + (mainRooms[i].doorPositions[j].corridor.width),
                        mainRooms[i].doorPositions[j].corridor.y,
                        -mainRooms[i].doorPositions[j].corridor.width,
                        mainRooms[i].doorPositions[j].corridor.height);
                    }
                    if (mainRooms[i].doorPositions[j].corridor.height < 0)
                    {
                        mainRooms[i].doorPositions[j].corridor =
                        new Rect(
                        mainRooms[i].doorPositions[j].corridor.x,
                        mainRooms[i].doorPositions[j].corridor.y + (mainRooms[i].doorPositions[j].corridor.height),
                        mainRooms[i].doorPositions[j].corridor.width,
                        -mainRooms[i].doorPositions[j].corridor.height);
                    }

                    mainRooms[i].doorPositions[j].corridor = new Rect(mainRooms[i].doorPositions[j].corridor.x - lowestX, mainRooms[i].doorPositions[j].corridor.y - lowestY, mainRooms[i].doorPositions[j].corridor.width, mainRooms[i].doorPositions[j].corridor.height);
                }
            }

            // ------------------------------ //

            #region Set Tilemap Size to Delaunay Dungeon Size

            int highestX = 0;
            int highestY = 0;

            // get x & y size
            for (int i = 0; i < mainRooms.Count; i++)
            {
                if (mainRooms[i].position.x + mainRooms[i].size.x > highestX)
                    highestX = (int)mainRooms[i].position.x + (int)mainRooms[i].size.x;

                if (mainRooms[i].position.y + mainRooms[i].size.y > highestY)
                    highestY = (int)mainRooms[i].position.y + (int)mainRooms[i].size.y;
            }

            highestX += 3;
            highestY += 3;

            // set tilemap
            MasMan.GridMan.tilesmap = new float[highestX, highestY];
            //MasMan.GridMan.grid.UpdateGrid(MasMan.GridMan.tilesmap);

            MasMan.GridMan.CreateMap();
            #endregion

            // ------------------- //

            for (int i = 0; i < mainRooms.Count; i++)
            {
                for (int x = 0; x < mainRooms[i].size.x; x++)
                {
                    for (int y = 0; y < mainRooms[i].size.y; y++)
                    {
                        //MasMan.GridMan.FindGridPiece((int)mainRooms[i].position.x + x, (int)mainRooms[i].position.y + y).MakePassable();
                        MasMan.GridMan.tilesmap[(int)mainRooms[i].position.x + x, (int)mainRooms[i].position.y + y] = 1;
                    }
                }
            }

            for (int i = 0; i < mainRooms.Count; i++)
            {
                for (int j = 0; j < mainRooms[i].doorPositions.Count; j++)
                {
                    //Debug.Log("room " + i + "|" + "door " + j);
                    for (int x = 0; x < mainRooms[i].doorPositions[j].corridor.size.x; x++)
                    {
                        for (int y = 0; y < mainRooms[i].doorPositions[j].corridor.size.y; y++)
                        {
                            int cx = (int)mainRooms[i].doorPositions[j].corridor.x + x;
                            int cy = (int)mainRooms[i].doorPositions[j].corridor.y + y;

                            //Debug.Log(mainRooms[i].doorPositions[j].corridorDir.ToString());
                            //Debug.Log(cx + " " + cy);

                            //MasMan.GridMan.FindGridPiece(cx, cy).MakePassable();
                            MasMan.GridMan.tilesmap[cx, cy] = 1;
                        }
                    }
                }
            }
            #endregion

            #region Show border and visible Grid Pieces
            for (int x = 0; x < MasMan.GridMan.tilesmap.GetLength(0); x++)
            {
                for (int y = 0; y < MasMan.GridMan.tilesmap.GetLength(1); y++)
                {
                    if (x == 0 || x == MasMan.GridMan.tilesmap.GetLength(0) - 1 || y == 0 || y == MasMan.GridMan.tilesmap.GetLength(1) - 1)
                        MasMan.GridMan.FindGridPiece(x, y).MakeImpassable();

                    if (MasMan.GridMan.tilesmap[x, y] == 1)
                        MasMan.GridMan.FindGridPiece(x, y).MakePassable();
                }
            }

            for (int x = 1; x < MasMan.GridMan.tilesmap.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < MasMan.GridMan.tilesmap.GetLength(1) - 1; y++)
                {
                    bool makeVisible = false;

                    if (MasMan.GridMan.tilesmap[x, y] == 0)
                    {
                        if (MasMan.GridMan.tilesmap[x, y + 1] == 1)
                            makeVisible = true;
                        else if (MasMan.GridMan.tilesmap[x, y - 1] == 1)
                            makeVisible = true;
                        else if (MasMan.GridMan.tilesmap[x + 1, y] == 1)
                            makeVisible = true;
                        else if (MasMan.GridMan.tilesmap[x - 1, y] == 1)
                            makeVisible = true;
                    }

                    if (makeVisible)
                        MasMan.GridMan.FindGridPiece(x, y).MakeImpassable();
                }
            }
            #endregion

            #region Place Player
            Actor.player.startingPosition = new Vector2Int(
                Random.Range(
                    (int)mainRooms[0].position.x,
                    (int)mainRooms[0].position.x + (int)mainRooms[0].rect.width),
                Random.Range(
                    (int)mainRooms[0].position.y,
                    (int)mainRooms[0].position.y + (int)mainRooms[0].rect.height));
            Actor.player.MoveToStartingPosition();
            #endregion

            #region Place Stairs

            GameObject badGuyInstance = Instantiate(MasMan.PreMan.badGuy);
            badGuyInstance.SetActive(true);
            BadGuy badGuy = badGuyInstance.GetComponent<BadGuy>();
            badGuy.startingPosition = Actor.player.startingPosition;
            badGuy.MoveToStartingPosition();

            // place stairs
            Actor.stairsUp.startingPosition = new Vector2Int(
    Random.Range(
        (int)mainRooms[mainRooms.Count - 1].position.x,
        (int)mainRooms[mainRooms.Count - 1].position.x + (int)mainRooms[mainRooms.Count - 1].rect.width),
    Random.Range(
        (int)mainRooms[mainRooms.Count - 1].position.y,
        (int)mainRooms[mainRooms.Count - 1].position.y + (int)mainRooms[mainRooms.Count - 1].rect.height));
            Actor.stairsUp.MoveToStartingPosition();

            #endregion

            // consider and create room types

            // check to see if player can access the stairs, if not reroll, as it turns out the generation is imperfect and will rarely create rooms with no connection ; consider checking each room for connections

            // distribute alloted enemies using perlin and room type; maybe with cr system

            MasMan.GridMan.grid.UpdateGrid(MasMan.GridMan.tilesmap);
        }

        public void StartTriangulation()
        {
            if (dg == null)
                return;
            mainRooms.Clear();
            float mean = dg.rooms.Average(o => o.rect.size.sqrMagnitude);
            var roomFromTheBiggest = dg.rooms.OrderBy(o => o.rect.size.sqrMagnitude).ToList();
            roomFromTheBiggest.Reverse();
            foreach (Room r in roomFromTheBiggest)
            {
                if (r.rect.size.sqrMagnitude >= mean)
                {
                    mainRooms.Add(r);
                }
                if (mainRooms.Count >= dg.rooms.Count * mainRoomSize)
                    break;
            }

            List<Vector2> m_points = mainRooms.Select(o => o.rect.center).ToList();
            List<uint> colors = new List<uint>();
            foreach (Vector2 v in m_points)
                colors.Add(0);

            v = new Voronoi(m_points, colors, new Rect(0, 0, -999999, 999999));

            m_delaunayTriangulation = v.DelaunayTriangulation();
            m_spanningTree = v.SpanningTree(KruskalType.MINIMUM);

            List<LineSegment> trisLeft = new List<LineSegment>();
            foreach (LineSegment d in m_delaunayTriangulation)
            {
                bool safeToAdd = true;
                foreach (LineSegment s in m_spanningTree)
                {
                    if ((d.p0 == s.p0 && d.p1 == s.p1) || (d.p0 == s.p1 && d.p1 == s.p0))
                    {
                        safeToAdd = false;
                        break;
                    }
                }
                if (safeToAdd)
                    trisLeft.Add(d);
            }

            trisLeft = trisLeft.OrderBy(o => (Vector2.SqrMagnitude((Vector2)(o.p0 - o.p1)))).ToList();
            for (int i = 0; i < (int)(trisLeft.Count * keepConnection); i++)
            {
                m_spanningTree.Add(trisLeft[i]);
            }

            dg.roomConnection.Clear();
            foreach (LineSegment l in m_spanningTree)
            {
                if (dg.roomConnection.ContainsKey(l))
                    continue;

                Room r1 = null, r2 = null;
                foreach (Room r in mainRooms)
                {
                    if (r.rect.center == l.p0)
                        r1 = r;
                    else if (r.rect.center == l.p1)
                        r2 = r;
                }

                if (r1 == null || r2 == null)
                {
#if DEBUG_MODE
                    Debug.Log("Dude, something doesn't right! Room is not detected in triangulation! Check here");
#endif
                }
                else
                {
                    dg.roomConnection.Add(l, new List<Room>(2) { r1, r2 });
                }
            }
        }

        public void DoGeneration()
        {
            dg.StartRoomGeneration();
            StartTriangulation();
            dg.StartCorridorsGeneration(mainRooms);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
#if DEBUG_MODE
            Gizmos.color = Color.magenta;
            if (m_delaunayTriangulation != null && drawTriangulation)
            {
                for (int i = 0; i < m_delaunayTriangulation.Count; i++)
                {
                    Vector2 left = (Vector2)m_delaunayTriangulation[i].p0;
                    Vector2 right = (Vector2)m_delaunayTriangulation[i].p1;
                    Gizmos.DrawLine((Vector3)left, (Vector3)right);
                }
            }
            if (m_spanningTree != null && drawSpanningTree)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < m_spanningTree.Count; i++)
                {
                    LineSegment seg = m_spanningTree[i];
                    Vector2 left = (Vector2)seg.p0;
                    Vector2 right = (Vector2)seg.p1;
                    Gizmos.DrawLine((Vector3)left, (Vector3)right);
                }
            }
            for (int i = 0; i < mainRooms.Count; i++)
            {
                Gizmos.color = Color.magenta;
                Vector2 bottomLeft = mainRooms[i].rect.position;
                Vector2 topRight = mainRooms[i].rect.position + mainRooms[i].rect.size;
                Vector2 topLeft = bottomLeft + Vector2.up * mainRooms[i].rect.height;
                Vector2 bottomRight = bottomLeft + Vector2.right * mainRooms[i].rect.width;

                Gizmos.DrawLine(bottomLeft, bottomRight);
                Gizmos.DrawLine(bottomLeft, topLeft);
                Gizmos.DrawLine(topRight, topLeft);
                Gizmos.DrawLine(topRight, bottomRight);

                UnityEditor.Handles.Label(topLeft + Vector2.right, mainRooms[i].id.ToString());

                Gizmos.color = Color.red;
                foreach (Door r in mainRooms[i].doorPositions)
                {
                    Gizmos.color = Color.green;
                    bottomLeft = r.position;
                    topRight = r.position + r.size;
                    topLeft = bottomLeft + Vector2.up * r.size.y;
                    bottomRight = bottomLeft + Vector2.right * r.size.x;

                    Gizmos.DrawLine(bottomLeft, bottomRight);
                    Gizmos.DrawLine(bottomLeft, topLeft);
                    Gizmos.DrawLine(topRight, topLeft);
                    Gizmos.DrawLine(topRight, bottomRight);

                    Gizmos.color = Color.red;
                    bottomLeft = r.corridor.position;
                    topRight = r.corridor.position + r.corridor.size;
                    topLeft = bottomLeft + Vector2.up * r.corridor.height;
                    bottomRight = bottomLeft + Vector2.right * r.corridor.width;

                    Gizmos.DrawLine(bottomLeft, bottomRight);
                    Gizmos.DrawLine(bottomLeft, topLeft);
                    Gizmos.DrawLine(topRight, topLeft);
                    Gizmos.DrawLine(topRight, bottomRight);
                }
            }
#endif
        }
#endif
    }
}
