using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private LayerMask bottomColliderMask;

    // Start is called before the first frame update
    void Start()
    {
        bottomColliderMask = LayerMask.NameToLayer("BottomCollider");
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: remove this logic when other components are setup
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == bottomColliderMask)
                    ObjectFactory.Instance.GenerateObject(hit.point);
            }

        }
    }
}
