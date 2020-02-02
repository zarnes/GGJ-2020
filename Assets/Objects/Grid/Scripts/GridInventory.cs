using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInventory : MonoBehaviour
{
    public GridType Type;

    private List<GridObject> _objects;
    private GridSystem _myGrid;

    private void Awake()
    {
        _objects = new List<GridObject>();
    }

    void Start()
    {
        _myGrid = GetComponent<GridSystem>();
    }
    
    public bool AddObject(GridObject gObj, Vector2Int coords, bool ignoreCollide = false)
    {
        if (!ignoreCollide)
        {
            GridObject collided = Collide(gObj, coords);
            if (collided != null)
                return false;
        }
        
        gObj.Position = coords;
        _objects.Add(gObj);
        // TODO feedback add
        return true;
    }

    public bool RemoveObject(GridObject gObj, bool destroy)
    {
        bool removed = _objects.Remove(gObj);
        // TODO feedback remove if rmeoved
        if (removed && destroy)
            Destroy(gObj.gameObject);
        return removed;
    }

    internal void FlushItems()
    {
        Debug.Log("Flushing " + _objects.Count + " items in grid inventory " + name, gameObject);
        if (_objects.Count <= 0)
            return;

        for (int i = _objects.Count - 1; i >= 0; --i)
        {
            RemoveObject(_objects[i], true);
        }
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

        GridObject collided = Collide(gObj, coords);
        if (collided != null)
        {
            Debug.Log("Colliding on end drag with " + collided.name);
            return RecipeManager.Instance.ApplyRecipe(gObj, collided);
            // TODO feedback collide, and eventual interaction
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

            Vector2Int oldPosition = gObj.Position;

            inventory.RemoveObject(gObj, false);
            AddObject(gObj, coords);
            
            if (inventory.Type == GridType.Input)
            {
                print("Duplicte");
                ObjectFactory.Instance.GenerateObject(GridManager.Instance.GetInputGridSystem(), oldPosition, gObj.Data);
            }
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
            if (collided != null && collided != gObj)
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

    public GridObject GetObjectWithData(GridObjectData data)
    {
        return _objects.Find(o => o.Data == data);
    }

    public enum GridType
    {
        Input,
        WorkBench,
        Output
    }
}
