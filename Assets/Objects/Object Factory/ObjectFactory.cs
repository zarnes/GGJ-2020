using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    public static ObjectFactory Instance;

    private bool IsObjectBeingTrash = false;
    private GridObject ObjectBeingTrash = null;
    private float ObjectTrashTimer = 0f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1) && IsObjectBeingTrash)
        {
            CancelTrashObject();
        }
    }
    
    public void GenerateObject(Vector3 objWorldPosition, GridObjectData data, bool ignoreCollide = false)
    {
        GridSystem gSysteme;
        Vector2Int gPos;

        if (GridManager.Instance.GetGridCoords(objWorldPosition, out gSysteme, out gPos))
        {
            Vector3 worldGridCellPos = gSysteme.GridToWorld(gPos);
            Quaternion sampleOverrideRotation = Quaternion.Euler(-90f, 0f, 0f);
            GameObject obj = Instantiate(data.Prefab, worldGridCellPos, sampleOverrideRotation);

            GridObject gObj = obj.GetComponent<GridObject>();
            gObj.InitializeFromDataFile(data);

            // TODO: Check return value when using this
            gSysteme.Inventory.AddObject(obj.GetComponent<GridObject>(), gPos, ignoreCollide);
        }
    }

    public void DeleteCurrentTrashObject()
    {
        GridInventory gInventory;
        gInventory = GridManager.Instance.FindGridWithObject(ObjectBeingTrash);

        if (gInventory)
        {
            if (!gInventory.RemoveObject(ObjectBeingTrash, true))
                Debug.LogError("Cannot find the item to delete on grid array");
        }
        else
            Debug.LogError("You have try to delete object that doesn't exist on this grid anymore");

        IsObjectBeingTrash = false;
        ObjectBeingTrash = null;
        ObjectTrashTimer = 0f;
    }

    public void BeginTrashObject(GridObject obj)
    {
        ObjectBeingTrash = obj;

        ObjectBeingTrash.cdMng.OnFinish += DeleteCurrentTrashObject;

        ObjectTrashTimer = Time.time + ObjectBeingTrash.TimeToDestroy;
        IsObjectBeingTrash = true;
        ObjectBeingTrash.LaunchTrashCooldownFeedback();
    }

    public void CancelTrashObject()
    {
        IsObjectBeingTrash = false;

        ObjectBeingTrash.cdMng.OnFinish -= DeleteCurrentTrashObject;

        ObjectBeingTrash.CancelTrashCooldown();
        ObjectBeingTrash = null;
        ObjectTrashTimer = 0f;
    }
}
