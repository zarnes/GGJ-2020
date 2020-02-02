using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset;
    
    [ReadOnly]
    public Vector2Int Position;
    [ReadOnly]
    public float TimeToDestroy;
    [ReadOnly]
    public float TimeToRespawn;

    [SerializeField]
    private bool MenuDragItem;

    [SerializeField, ReadOnly]
    private bool IsDragable;

    [ReadOnly]
    public Vector3 initialDragPosition;
    
    public GridObjectData Data { get; private set; }

    [SerializeField]
    public CooldownManager cdMng;
    
    [Header("Dont touch this")]
    
    [Header("Dont touch this")]
    public List<Vector2Int> CoordinatesUsed;

    public void InitializeFromDataFile(GridObjectData data, bool shouldBeOnCooldown = false)
    {
        Data = data;
        CoordinatesUsed = data.CoordinatesUsed;
        TimeToDestroy = data.TimeToDestroy;
        TimeToRespawn = data.TimeToRespawn;
        gameObject.name = data.name;
        transform.position -= _offset;

        if (shouldBeOnCooldown)
        {
            LaunchSpawnCooldownFeedback();
            IsDragable = false;
        }
        else
            IsDragable = true;
    }

    public void LaunchSpawnCooldownFeedback()
    {
        cdMng.gameObject.SetActive(true);
        cdMng.Launch(TimeToRespawn);
        cdMng.OnFinish += MakeDragable;
    }

    public void MakeDragable()
    {
        IsDragable = true;
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
        if (!IsDragable)
            return;

        GridSystem gSystem;
        GridManager.Instance.GetGridCoords(transform.position, out gSystem, out _);
        initialDragPosition = transform.position;
        bool registered = gSystem.Inventory.StartMove(this);
    }

    public void OnMouseDrag()
    {
        if (!IsDragable)
            return;

        Vector3 mosPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mosPos);
        worldPos.z = 0f;

        transform.position = worldPos;

        if (MenuDragItem)
            MenuManager.Instance.DragDrop(MenuManager.DragDropState.Drop);
    }

    void OnMouseUp()
    {
        if (!IsDragable)
            return;

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
            transform.position = initialDragPosition;
            GridManager.Instance.UnregisterDragged();
        }

        initialDragPosition = Vector3.zero;

        if (MenuDragItem)
            MenuManager.Instance.DragDrop(MenuManager.DragDropState.Drag);
    }

    private void OnMouseOver()
    {
        GameCanvas.Instance?.UpdateInformation(Data.Name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position + _offset, .5f);
    }
}
