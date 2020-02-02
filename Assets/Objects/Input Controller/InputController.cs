using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private LayerMask objectColliderMask;

    void Start()
    {
        objectColliderMask = LayerMask.NameToLayer("ObjectCollider");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == objectColliderMask)
                {
                    GridObject obj = hit.transform.gameObject.GetComponent<GridObject>();

                    GridManager gMng = GridManager.Instance;

                    if (gMng.FindGridWithObject(obj) != gMng.GetInputGridSystem().Inventory && obj.Data.IsDestroyable)
                    {
                        ObjectFactory.Instance.BeginTrashObject(obj);
                    }

                }
            }
        }
    }
}