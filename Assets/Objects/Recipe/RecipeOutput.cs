using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeOutput
{
    public GridObjectData Object;
    public int InputObjectIndex;
    public float TimeToCraft = 0;
    public Vector2Int ObjectRelativePosition;
}
