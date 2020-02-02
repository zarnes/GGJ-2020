using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveFeedback : MonoBehaviour
{
    private ObjectiveConfiguration _configuration;

    public void Init(ObjectiveConfiguration configuration)
    {
        _configuration = configuration;
        name = "Objective : " + configuration.Object.name;
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = configuration.FeedbackSprite;
        transform.GetChild(0).GetChild(0).localScale = Vector3.one * configuration.SpriteScale;
    }

    void OnMouseUp()
    {
        GameCanvas.Instance.OpenScroll(_configuration);
    }

    public void Complete()
    {
        GetComponentInChildren<Animator>().SetTrigger("Destroy");
        Destroy(gameObject, 2f);
    }
}
