﻿using System;
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
    }

    void OnMouseUp()
    {
        GameCanvas.Instance.OpenScroll(_configuration);
    }
}
