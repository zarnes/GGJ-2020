using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform CellsParent;
    public GameObject CellPrefab;

    [Space]
    public List<GridSystem> Grids;
    
    // Start is called before the first frame update
    void Awake()
    {
        foreach(GridSystem grid in Grids)
        {
            for (int x = 0; x < grid.GridSize.x; ++x)
            {
                for (int y = 0; y < grid.GridSize.y; ++y)
                {
                    Instantiate(CellPrefab, grid.GridToWorld(new Vector2Int(x, y)), Quaternion.identity, CellsParent);
                }
            }
        }
    }
}
