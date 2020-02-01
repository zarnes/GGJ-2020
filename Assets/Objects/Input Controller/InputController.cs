using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private LayerMask bottomColliderMask;
    private LayerMask objectColliderMask;

    public GridObjectData A;
    public GridObjectData B;
    
    void Start()
    {
        bottomColliderMask = LayerMask.NameToLayer("BottomCollider");
        objectColliderMask = LayerMask.NameToLayer("ObjectCollider");
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: remove this logic when other components are setup
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == bottomColliderMask)
                {
                    GridObjectData data = Input.GetMouseButtonDown(0) ? A : B;
                    ObjectFactory.Instance.GenerateObject(hit.point, data);
                }
                else if (hit.transform.gameObject.layer == objectColliderMask)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        GridObject obj = hit.transform.gameObject.GetComponent<GridObject>();
                        obj.LaunchTrashCooldownFeedback();
                        ObjectFactory.Instance.BeginTrashObject(obj);
                    }
                }
            }
        }
    }
}
