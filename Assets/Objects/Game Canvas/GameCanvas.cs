using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Text TimerText;
    [SerializeField]
    private Animator TimerAnimator;

    private int lastTimerDisplayed;

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

    internal void UpdateTimer(float timeRemaining, bool animate = true)
    {
        if ((int)timeRemaining == lastTimerDisplayed)
            return;

        lastTimerDisplayed = (int)timeRemaining;

        string minutes = (lastTimerDisplayed / 60).ToString();
        if (minutes.Length < 2)
            minutes = "0" + minutes;

        string seconds = (lastTimerDisplayed % 60).ToString();
        if (seconds.Length < 2)
            seconds = "0" + seconds;

        TimerText.text = minutes + ":" + seconds;
        if (animate)
            TimerAnimator.SetTrigger("Wobble");
    }
}
