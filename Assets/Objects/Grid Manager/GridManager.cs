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

        // TODO keep this ? See GD
        if (grid.Type == GridInventory.GridType.Output)
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
}
