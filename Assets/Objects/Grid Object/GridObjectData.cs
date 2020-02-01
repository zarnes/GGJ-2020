﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object Data", menuName = "Inventory System/Object", order =  0)]
public class GridObjectData : ScriptableObject
{
    public GameObject Prefab;
    public List<Vector2Int> CoordinatesUsed;
}
