using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    public static ObjectFactory Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
}
