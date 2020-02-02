using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public List<Vector2Int> CoordinatesUsed;
    public Vector2Int Position;
    public float TimeToDestroy;

    public Vector3 initialDragPosition;
    
    public GridObjectData Data { get; private set; }

    [SerializeField]
    public CooldownManager cdMng;
    
    public void InitializeFromDataFile(GridObjectData data)
    {
        Data = data;
        CoordinatesUsed = data.CoordinatesUsed;
        TimeToDestroy = data.TimeToDestroy;
        gameObject.name = data.name;
    }

    public void LaunchTrashCooldownFeedback()
    {
        cdMng.gameObject.SetActive(true);
        cdMng.Launch(TimeToDestroy);
    }

    public void CancelTrashCooldown()
    {
        cdMng.Stop();
        cdMng.gameObject.SetActive(false);
    }

    void OnMouseDown()
    {
        GridSystem gSystem;
        GridManager.Instance.GetGridCoords(transform.position, out gSystem, out _);
        initialDragPosition = transform.position;
        bool registered = gSystem.Inventory.StartMove(this);
        Debug.Log("Registered success: " + registered);
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

        if (GridManager.Instance.GetGridCoords(transform.position, out gSystem, out gCoords) && gSystem.Inventory.Type != GridInventory.GridType.Input)
        {
            Vector3 worldPos = gSystem.GridToWorld(gCoords);
            transform.position = gSystem.Inventory.EndMove(gCoords) ? worldPos : initialDragPosition;
        }
        else
        {
            transform.position = initialDragPosition;
            GridManager.Instance.UnregisterDragged();
        }

        initialDragPosition = Vector3.zero;
    }
}
