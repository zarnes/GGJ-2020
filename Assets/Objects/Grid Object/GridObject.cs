using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset;
    
    internal List<Vector2Int> CoordinatesUsed;
    public Vector2Int Position;
    public float TimeToDestroy;

    [SerializeField]
    private bool MenuDragItem;

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
        transform.position -= _offset;
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
    }

    public void OnMouseDrag()
    {
        Vector3 mosPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mosPos);
        worldPos.z = 0f;

        transform.position = worldPos;

        if (MenuDragItem)
            MenuManager.Instance.DragDrop(MenuManager.DragDropState.Drop);
    }

    void OnMouseUp()
    {
        GridSystem gSystem;
        Vector2Int gCoords;

        if (GridManager.Instance.GetGridCoords(transform.position, out gSystem, out gCoords) && gSystem.Inventory.Type != GridInventory.GridType.Input)
        {
            Vector3 worldPos = gSystem.GridToWorld(gCoords);
            transform.position = gSystem.Inventory.EndMove(gCoords) ? worldPos - _offset : initialDragPosition;
            //transform.position -= _offset;

            if (MenuDragItem)
                MenuManager.Instance.TestDragNDropUnderstood(gSystem);
        }
        else
        {
            transform.position = initialDragPosition - _offset;
            GridManager.Instance.UnregisterDragged();
        }

        initialDragPosition = Vector3.zero;

        if (MenuDragItem)
            MenuManager.Instance.DragDrop(MenuManager.DragDropState.Drag);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position + _offset, .5f);
    }
}
