using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    internal static MenuManager Instance;

    [SerializeField]
    private Animator DragNDropAnim;
    [SerializeField]
    private GridObject MenuDrag;
    [SerializeField]
    private GridSystem DragGrid;

    [SerializeField]
    [Space]
    private GridSystem LevelsGrid;
    [SerializeField]
    private List<LevelObjectConfiguration> LevelsObjectsConfig;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        DragGrid.Inventory.AddObject(MenuDrag, Vector2Int.zero, true);
    }

    public void TestDragNDropUnderstood(GridSystem gSystem)
    {
        if (gSystem != DragGrid)
            DragDrop(DragDropState.Hidden);
    }

    public void DragDrop(DragDropState state)
    {
        switch (state)
        {
            case DragDropState.Drag:
                DragNDropAnim.SetBool("Drag", false);
                break;
            case DragDropState.Drop:
                DragNDropAnim.SetBool("Drag", true);
                break;
            case DragDropState.Hidden:
                DragNDropAnim.SetTrigger("Hide");
                break;
        }
    }

    public enum DragDropState
    {
        Drag,
        Drop,
        Hidden
    }
}

[System.Serializable]
public class LevelObjectConfiguration
{
    public GridObject Object;
    public int LevelIndex;
    public bool Quit;
}
