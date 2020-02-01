using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    public static ObjectFactory Instance;

    // TODO: remove this logic when other components are setup
    [SerializeField]
    private GameObject SampleObject;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
    }

    void Update()
    {

    }

    public void GenerateObject(Vector3 objWorldPosition)
    {
        GridSystem gSysteme;
        Vector2Int gPos;

        if (GridManager.Instance.GetGridCoords(objWorldPosition, out gSysteme, out gPos))
        {
            Vector3 worldGridCellPos = gSysteme.GridToWorld(gPos);
            Quaternion sampleOverrideRotation = Quaternion.Euler(-90f, 0f, 0f);
            GameObject obj = GameObject.Instantiate(SampleObject, worldGridCellPos, sampleOverrideRotation);

            // TODO: Check return value when using this
            gSysteme.Inventory.AddObject(obj.GetComponent<GridObject>(), gPos);
        }
    }
}
