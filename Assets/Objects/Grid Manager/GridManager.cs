using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField]
    private List<GridSystem> _grids;
    private GridObject _currentDragged = null;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool GetGridCoords(Vector3 worldCoords, out GridSystem grid, out Vector2Int coords)
    {
        foreach(GridSystem gs in _grids)
        {
            coords = gs.WorldToGrid(worldCoords);
            if (coords.x != -1 && coords.y != -1)
            {
                grid = gs;
                return true;
            }
        }

        grid = null;
        coords = new Vector2Int();
        return false;
    }

    public bool RegisterDragged(GridObject gridObj, GridInventory grid)
    {
        if (_currentDragged != null)
            return false;
        

        _currentDragged = gridObj;
        return true;
    }

    public GridObject UnregisterDragged()
    {
        GridObject oldDragged = _currentDragged;
        _currentDragged = null;
        return oldDragged;
    }

    public GridInventory FindGridWithObject(GridObject gObj)
    {
        foreach(GridSystem gs in _grids)
        {
            if (gs.Inventory.HasObject(gObj))
                return gs.Inventory;
        }

        return null;
    }

    public GridSystem GetInputGridSystem()
    {
        return _grids[0];
    }

    public bool SpawnScrapInRandomZone(GridObjectData data, Vector3 epicenter, float Range)
    {
        int tries = 10;
        while (tries >= 0)
        {
            --tries;
            Vector2 randCircle = Random.insideUnitCircle * Range;
            Vector3 position = epicenter + new Vector3(randCircle.x, randCircle.y);
            GridSystem grid;
            Vector2Int gCoords;
            if (GetGridCoords(position, out grid, out gCoords))
            {
                if (grid.Inventory.Type == GridInventory.GridType.WorkBench)
                {
                    if (ObjectFactory.Instance.GenerateObject(position, data))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
