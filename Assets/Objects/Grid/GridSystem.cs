using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    internal static GridSystem Instance;

    public Vector2 GridSize;
    public Vector2 CellSize;

    public Vector2Int MouseCoords;
    private Camera _cam;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (_cam == null)
            _cam = Camera.main;

        Vector3 mouseCoords = _cam.ScreenToWorldPoint(Input.mousePosition);

        MouseCoords = WorldToGrid(mouseCoords);
    }

    public Vector2Int WorldToGrid(Vector3 worldCoords)
    {
        worldCoords -= transform.position;
        worldCoords /= CellSize;
        return new Vector2Int((int)worldCoords.x, (int)worldCoords.y);
    }

    public Vector3 GridToWorld(Vector2Int gridCoords)
    {
        Vector3 worldCoord = new Vector3(gridCoords.x * CellSize.x, gridCoords.y * CellSize.y);
        return worldCoord + transform.position + new Vector3(CellSize.x * .5f, CellSize.y * .5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        for (int x = 0; x < GridSize.x; ++x)
        {
            for (int y = 0; y < GridSize.y; ++y)
            {
                Vector3 topRight = transform.position + new Vector3(CellSize.x * (x + 1), CellSize.y * (y + 1));
                Vector3 bottomLeft = transform.position + new Vector3(CellSize.x * x, CellSize.y * y);
                Vector3 bottomRight = transform.position + new Vector3(CellSize.x * (x + 1), CellSize.y * y);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(topRight, bottomRight);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(bottomRight, bottomLeft);

            }
        }

        Gizmos.color = Color.grey;
        Vector3 topLeftG = transform.position + new Vector3(0, CellSize.y * GridSize.y);
        Vector3 topRightG = transform.position + new Vector3(CellSize.x * GridSize.x, CellSize.y * GridSize.y);

        Gizmos.DrawLine(transform.position, topLeftG);
        Gizmos.DrawLine(topLeftG, topRightG);
    }
}