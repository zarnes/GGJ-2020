﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeInput
{
    public GridObjectData Object;
    public CraftBehavior Behavior;
    public bool CanBeTargetObject;

    public enum CraftBehavior
    {
        PlaceBack,
        Destroy
    }
}
