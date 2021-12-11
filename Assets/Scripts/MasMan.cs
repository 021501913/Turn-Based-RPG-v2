using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Master Manager, master of all managers
/// </summary>
public class MasMan : MonoBehaviour
{
    private static MiniGameManager _miniGameManager;

    /// <summary>
    /// Handles Minigames
    /// </summary>
    public static MiniGameManager MGMan
    {
        get
        {
            if (_miniGameManager == null)
            {
                _miniGameManager = GameObject.FindObjectOfType<MiniGameManager>();
            }

            return _miniGameManager;
        }
    }

    private static GridManager _gridManager;
    /// <summary>
    /// Handles de/construction of the physical grid
    /// </summary>
    public static GridManager GridMan
    {
        get
        {
            if (_gridManager == null)
            {
                _gridManager = GameObject.FindObjectOfType<GridManager>();
            }

            return _gridManager;
        }
    }

    private static DI.DungeonGenerator.DelaunayDungeonGenerator _delaunayDungeonGenerator;
    /// <summary>
    /// Handles random generation of the grid/rooms/enemies, as well as actor positions
    /// </summary>
    public static DI.DungeonGenerator.DelaunayDungeonGenerator dungeonGenerator
    {
        get
        {
            if (_delaunayDungeonGenerator == null)
            {
                _delaunayDungeonGenerator = GameObject.FindObjectOfType<DI.DungeonGenerator.DelaunayDungeonGenerator>();
            }

            return _delaunayDungeonGenerator;
        }
    }

    private static PrefabManager _prefabManager;
    /// <summary>
    /// Prefabs dur
    /// </summary>
    public static PrefabManager PreMan
    {
        get
        {
            if (_prefabManager == null)
            {
                _prefabManager = GameObject.FindObjectOfType<PrefabManager>();
            }

            return _prefabManager;
        }
    }

    private static InventoryManager _inventoryManager;
    /// <summary>
    /// Handles Inventory (slots and itemDTOs)
    /// </summary>
    public static InventoryManager InventoryManager
    {
        get
        {
            if (_inventoryManager == null)
            {
                _inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
            }

            return _inventoryManager;
        }
    }
}
