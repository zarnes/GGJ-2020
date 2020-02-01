﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelConfiguration LevelConfiguration;

    [Space]
    public ObjectiveManager ObjectiveManager;
    public GameCanvas GameCanvas;

    private bool _firstObjectiveFinished = false;
    [SerializeField][ReadOnly]
    private float TimeRemaining;
    [SerializeField][ReadOnly]
    private float _nextSpawn;
    private float _currentRespawnTime;

    // Start is called before the first frame update
    void Start()
    {
        _currentRespawnTime = LevelConfiguration.RespawnTime;
        _nextSpawn = _currentRespawnTime;
        ObjectiveManager.LoadConfiguration(LevelConfiguration);
        TimeRemaining = LevelConfiguration.LevelTime;
        GameCanvas.UpdateTimer(TimeRemaining, false);
    }

    // Update is called once per frame
    void Update()
    {
        GameCanvas.UpdateTimer(TimeRemaining);

        if (_firstObjectiveFinished)
        {
            TimeRemaining -= Time.deltaTime;
            _nextSpawn -= Time.deltaTime;
        }

        if (ObjectiveManager.ActiveObjectives == 0)
        {
            // Objectives spawn faster if the objective is completed more quickly
            if (_firstObjectiveFinished && _currentRespawnTime > LevelConfiguration.MinimumRespawnTime)
            {
                _currentRespawnTime -= LevelConfiguration.TimeDecreasePerPerfect;
                if (_currentRespawnTime < LevelConfiguration.MinimumRespawnTime)
                    _currentRespawnTime = LevelConfiguration.MinimumRespawnTime;
            }
            else if (!_firstObjectiveFinished)
                _firstObjectiveFinished = true;

            SpawnRandomObjective();
        }
        else if (_nextSpawn <= 0)
        {
            SpawnRandomObjective();
        }

        if (TimeRemaining <= 0)
        {
            Debug.Log("TIME OUT");
            gameObject.SetActive(false);
        }
    }

    private void SpawnRandomObjective()
    {
        int rndIndex = Random.Range(0, LevelConfiguration.Objectives.Count - 1);
        ObjectiveConfiguration config = LevelConfiguration.Objectives[rndIndex];
        ObjectiveManager.SpawnObjective(config);
        _nextSpawn = _currentRespawnTime;
    }
}
