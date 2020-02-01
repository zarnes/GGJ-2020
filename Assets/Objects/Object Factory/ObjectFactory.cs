using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    // TODO: remove this logic when other components are setup
    [SerializeField]
    private GameObject SampleObject;
    
    void Start()
    {
        
    }

    void Update()
    {
        // TODO: remove this logic when other components are setup
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GenerateObject(hit.point);
            }
                
        }
    }

    void GenerateObject(Vector3 objWorldPosition)
    {
        GridSystem gSysteme;
        Vector2Int gPos;

        if (GridManager.Instance.GetGridCoords(objWorldPosition, out gSysteme, out gPos))
        {
            Vector3 worldGridCellPos = gSysteme.GridToWorld(gPos);
            Quaternion sampleRotation = Quaternion.Euler(-90f, 0f, 0f);
            GameObject.Instantiate(SampleObject, worldGridCellPos, sampleRotation);

            print("Grid type : " + gSysteme.Inventory.Type);
            print("gPos : " + gPos);
        }
    }
}
