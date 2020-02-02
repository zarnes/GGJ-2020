using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public Text FeedbackText;

    private LayerMask bottomColliderMask;
    private LayerMask objectColliderMask;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, objectColliderMask))
        {
            if (hit.transform.gameObject.layer == objectColliderMask)
            {
                Debug.Log(hit.transform.name);
                FeedbackText.text = hit.transform.name;
            }
        }
    }
}
