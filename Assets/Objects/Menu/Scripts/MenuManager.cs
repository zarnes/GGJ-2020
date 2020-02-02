using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    internal static MenuManager Instance;

    [SerializeField]
    private Animator _dragNDropAnim;
    [SerializeField]
    private GridObject _menuDrag;
    [SerializeField]
    private GridSystem _dragGrid;

    [SerializeField]
    [Space]
    private GridSystem _levelsGrid;
    [SerializeField]
    private List<LevelObjectConfiguration> _levelsObjectsConfig;

    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private float _finalCamSize;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        _levelsGrid.Inventory.InMenu = _dragGrid.Inventory.InMenu = true;

        _dragGrid.Inventory.AddObject(_menuDrag, Vector2Int.zero, true);

        foreach (LevelObjectConfiguration config in _levelsObjectsConfig)
        {
            _levelsGrid.Inventory.AddObject(config.Object, config.Position, true);
        }
    }

        public void TestDragNDropUnderstood(GridSystem gSystem)
    {
        if (gSystem != _dragGrid)
            DragDrop(DragDropState.Hidden);
    }

    public void DragDrop(DragDropState state)
    {
        switch (state)
        {
            case DragDropState.Drag:
                _dragNDropAnim.SetBool("Drag", false);
                break;
            case DragDropState.Drop:
                _dragNDropAnim.SetBool("Drag", true);
                break;
            case DragDropState.Hidden:
                _dragNDropAnim.SetTrigger("Hide");
                break;
        }
    }

    internal void Selected(GridObject collided)
    {
        foreach(LevelObjectConfiguration config in _levelsObjectsConfig)
        {
            if (config.Object == collided)
            {
                if (config.Quit)
                    Application.Quit();

                StartCoroutine(SelectLevel(collided.transform, config.LevelIndex));
            }
        }
    }

    private IEnumerator SelectLevel(Transform tf, int index)
    {
        float start = Time.time;
        float end = start + .7f;
        float current = start;

        float startSize = _camera.orthographicSize;
        Vector3 startPos = _camera.transform.position;
        Vector3 endPos = tf.position;
        endPos.z = startPos.z;

        bool soundA = false;
        bool soundB = false;
        bool soundC = false;

        while (current <= end)
        {
            current = Time.time;
            float percentage = Mathf.InverseLerp(start, end, current);
            _camera.orthographicSize = Mathf.Lerp(startSize, _finalCamSize, percentage);
            _camera.transform.position = Vector3.Lerp(startPos, endPos, percentage);

            if (percentage >= .0f && !soundA)
            {
                MusicManager.Instance.PlaySound("Book A");
                soundA = true;
            }
            else if (percentage >= .3f && !soundB)
            {
                MusicManager.Instance.PlaySound("Book B");
                soundB = true;
            }
            else if (percentage >= .6f && !soundC)
            {
                MusicManager.Instance.PlaySound("Book C");
                soundC = true;
            }

            yield return null;
        }

        LevelManager.Instance.LoadLevel(index);
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
    public Vector2Int Position;
}
