using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    internal static GameCanvas Instance;

    [SerializeField]
    private ObjectiveManager ObjectiveManager;
    [SerializeField]
    private GridInventory OutputInventory;

    [Space]
    [SerializeField]
    private RectTransform Scrollcontent;
    [SerializeField]
    private Animator ScrollAnimator;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ValidateOutput()
    {
        ObjectiveManager.ValidateObjectives(OutputInventory);
    }

    public void OpenScroll(ObjectiveConfiguration configuration)
    {
        if (Scrollcontent.childCount != 0)
            Destroy(Scrollcontent.GetChild(0).gameObject);

        Instantiate(configuration.ScrollInfos, Scrollcontent);
        ScrollAnimator.SetBool("Show", true);
    }

    public void CloseScroll()
    {
        ScrollAnimator.SetBool("Show", false);
    }
}
