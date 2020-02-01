using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    private SpriteMask mask;

    private bool IsStarted;
    private bool IsFinished;

    private float duration;
    private float EndTime;
    
    public delegate void CooldownAction();

    public event CooldownAction OnFinish;
    public event CooldownAction OnStart;

    void Awake()
    {
        mask = GetComponentInChildren<SpriteMask>();
    }

    public void Launch(float durationInSeconds)
    {
        IsStarted = true;
        duration = durationInSeconds;
        EndTime = Time.time + durationInSeconds;
        OnStart?.Invoke();
    }

    void Update()
    {
        if (IsStarted && !IsFinished)
        {
            if (EndTime + Mathf.Epsilon >= Time.time)
                mask.alphaCutoff = Mathf.Clamp01(duration - (EndTime - Time.time));
            else
            {
                IsFinished = true;
                OnFinish?.Invoke();
            }
            
        }
    }
}
