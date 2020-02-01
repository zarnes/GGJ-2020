using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public List<Vector2Int> CoordinatesUsed;
    public Vector2Int Position;

    public Vector3 initialDragPosition;

    public void InitializeFromDataFile(GridObjectData data)
    {
        CoordinatesUsed = data.CoordinatesUsed;
    }

    void OnMouseDown()
    {
        GridSystem gSystem;
        GridManager.Instance.GetGridCoords(transform.position, out gSystem, out _);
        initialDragPosition = transform.position;
        gSystem.Inventory.StartMove(this);
    }

    public void OnMouseDrag()
    {
        Vector3 mosPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mosPos);
        worldPos.z = 0f;

        transform.position = worldPos;
    }

    void OnMouseUp()
    {
        GridSystem gSystem;
        Vector2Int gCoords;

        if (GridManager.Instance.GetGridCoords(transform.position, out gSystem, out gCoords))
        {
            Vector3 worldPos = gSystem.GridToWorld(gCoords);

            transform.position = gSystem.Inventory.EndMove(gCoords) ? worldPos : initialDragPosition;
        }
        else
        {
            transform.position = initialDragPosition;
        }

        initialDragPosition = Vector3.zero;
    }
}
