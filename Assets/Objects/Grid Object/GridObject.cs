﻿using System.Collections;
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

    [SerializeField]
    private bool IsDragable;

    [ReadOnly]
    public Vector3 initialDragPosition;
    
    public GridObjectData Data { get; private set; }

    [SerializeField]
    public CooldownManager cdMng;
    
    [Header("Dont touch this")]
    public List<Vector2Int> CoordinatesUsed;

    [SerializeField]
    private MusicManager.SoundConfig _onSound;
    [SerializeField]
    private MusicManager.SoundConfig _offSound;

    public void InitializeFromDataFile(GridObjectData data, bool shouldBeOnCooldown = false)
    {
        Data = data;
        CoordinatesUsed = data.CoordinatesUsed;
        TimeToDestroy = data.TimeToDestroy;
        TimeToRespawn = data.TimeToRespawn;
        gameObject.name = data.name;
        transform.position -= _offset;

        _onSound = data.TakeOnSound;
        _offSound = data.TakeOffSound;

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
        cdMng.Stop();
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

        if (registered && _onSound.Clip != null)
            MusicManager.Instance.PlaySound(_onSound);
    }

    public void OnMouseDrag()
    {
        if (!IsDragable)
        {
            Input.ResetInputAxes();
            return;
        }

        Vector3 mosPos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mosPos);
        worldPos.z = 0f;
        worldPos -= _offset;

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

        if (GridManager.Instance.GetGridCoords(transform.position + _offset, out gSystem, out gCoords) && gSystem.Inventory.Type != GridInventory.GridType.Input)
        {
            Vector3 worldPos = gSystem.GridToWorld(gCoords);
            transform.position = gSystem.Inventory.EndMove(gCoords) ? worldPos - _offset : initialDragPosition;
            //transform.position -= _offset;

            if (MenuDragItem)
                MenuManager.Instance.TestDragNDropUnderstood(gSystem);

            if (_offSound.Clip != null)
                MusicManager.Instance.PlaySound(_offSound);
        }
        else
        {
            transform.position = initialDragPosition;
            GridManager.Instance.UnregisterDragged();
        }

        initialDragPosition = transform.position;

        if (MenuDragItem)
            MenuManager.Instance.DragDrop(MenuManager.DragDropState.Drag);
    }

    private void OnMouseOver()
    {
        if (GameCanvas.Instance != null)
            GameCanvas.Instance.UpdateInformation(Data.Name);
    }

    private void OnMouseExit()
    {
        if (GameCanvas.Instance != null)
            GameCanvas.Instance.UpdateInformation("");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position + _offset, .5f);
    }
}
