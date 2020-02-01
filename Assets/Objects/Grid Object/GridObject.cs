using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public List<Vector2Int> CoordinatesUsed;
    public Vector2Int Position;

    public void InitializeFromDataFile(GridObjectData data)
    {
        CoordinatesUsed = data.CoordinatesUsed;
    }


}
