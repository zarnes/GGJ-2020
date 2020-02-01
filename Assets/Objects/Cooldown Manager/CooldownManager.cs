using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    [SerializeField]
    private SpriteMask mask;

    private bool IsStarted;
    private bool IsFinished;

    private float duration;
    private float value;
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

    public void Stop()
    {
        IsStarted = false;
        EndTime = 0f;
    }

    void Update()
    {
        if (IsStarted && !IsFinished)
        {
            value += Time.deltaTime;

            float clampedValue = Mathf.Clamp(value, 0f, duration);
            
            if (EndTime >= Time.time)
                mask.alphaCutoff = clampedValue / duration;
            else
            {
                IsFinished = true;
                OnFinish?.Invoke();
                //this.enabled = false;
            }
            
        }
    }
}
