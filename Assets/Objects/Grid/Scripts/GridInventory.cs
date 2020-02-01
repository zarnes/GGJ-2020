using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInventory : MonoBehaviour
{
    public GridType Type;

    private List<GridObject> _objects;
    private GridSystem _myGrid;

    // Start is called before the first frame update
    void Start()
    {
        _objects = new List<GridObject>();
        _myGrid = GetComponent<GridSystem>();
    }
    
    public bool AddObject(GridObject gObj, Vector2Int coords)
    {
        GridObject collided = Collide(gObj, coords);
        if (collided != null)
            return false;

        gObj.Position = coords;
        _objects.Add(gObj);
        // TODO feedback add
        return true;
    }

    public bool RemoveObject(GridObject gObj)
    {
        bool removed = _objects.Remove(gObj);
        // TODO feedback remove if rmeoved
        return removed;
    }

    public bool StartMove(GridObject gObj)
    {
        if (!_objects.Contains(gObj))
            return false;

        // TODO feedback start drag
        return GridManager.Instance.RegisterDragged(gObj, this);
    }

    public bool EndMove(Vector2Int coords)
    {
        GridObject gObj = GridManager.Instance.UnregisterDragged();
        if (gObj == null)
        {
            Debug.LogWarning("Tried to end a move while no object was dragged", gameObject);
            return false;
        }

        if (!ObjectPositionValid(gObj, coords))
        {
            // TODO feedback not in grid
            return false;
        }

        else if (Collide(gObj, coords) != null)
        {
            // TODO feedback collide, and eventual interaction
            return false;
        }
        
        // If dragged object was not in the same grid
        if (!_objects.Contains(gObj))
        {
            GridInventory inventory = GridManager.Instance.FindGridWithObject(gObj);
            if (inventory == null)
            {
                Debug.LogError("Trying to move an object from an undefined grid");
                return false;
            }

            inventory.RemoveObject(gObj);
            AddObject(gObj, coords);
        }

        gObj.Position = coords;
        return true;
    }

    public GridObject GetObject(Vector2Int coords)
    {
        foreach(GridObject gObj in _objects)
        {
            foreach(Vector2Int relativeCoord in gObj.CoordinatesUsed)
            {
                Vector2Int finalCoord = gObj.Position + relativeCoord;
                if (finalCoord == coords)
                    return gObj;
            }
        }
        return null;
    }

    public GridObject Collide(GridObject gObj, Vector2Int coords)
    {
        foreach(Vector2Int relativeCoord in gObj.CoordinatesUsed)
        {
            Vector2Int finalCoord = coords + relativeCoord;
            GridObject collided = GetObject(finalCoord);
            if (collided != null)
                return collided;
        }

        return null;
    }

    public bool ObjectPositionValid(GridObject gObj, Vector2Int coords)
    {
        foreach(Vector2Int relativeCoord in gObj.CoordinatesUsed)
        {
            Vector2Int finalCoords = relativeCoord + coords;
            if (!_myGrid.IsCoordValid(finalCoords))
                return false;
        }

        return true;
    }

    public bool HasObject(GridObject gObj)
    {
        return _objects.Contains(gObj);
    }

    public enum GridType
    {
        Input,
        WorkBench,
        Output
    }
}
