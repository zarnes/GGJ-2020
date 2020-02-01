using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LevelConfiguration _levelConfiguration;

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
        _levelConfiguration = LevelManager.Instance.CurrentLevel;

        _currentRespawnTime = _levelConfiguration.RespawnTime;
        _nextSpawn = _currentRespawnTime;
        ObjectiveManager.LoadConfiguration(_levelConfiguration);
        TimeRemaining = _levelConfiguration.LevelTime;
        GameCanvas.UpdateTimer(TimeRemaining, 0, false);
    }

    // Update is called once per frame
    void Update()
    {
        GameCanvas.UpdateTimer(TimeRemaining, 1 - TimeRemaining / _levelConfiguration.LevelTime);

        if (_firstObjectiveFinished)
        {
            TimeRemaining -= Time.deltaTime;
            _nextSpawn -= Time.deltaTime;
        }

        if (ObjectiveManager.ActiveObjectives == 0)
        {
            // Objectives spawn faster if the objective is completed more quickly
            if (_firstObjectiveFinished && _currentRespawnTime > _levelConfiguration.MinimumRespawnTime)
            {
                _currentRespawnTime -= _levelConfiguration.TimeDecreasePerPerfect;
                if (_currentRespawnTime < _levelConfiguration.MinimumRespawnTime)
                    _currentRespawnTime = _levelConfiguration.MinimumRespawnTime;
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
            GameCanvas.EndGame(345, LevelManager.Instance.HasNextLevel(_levelConfiguration));
        }
    }

    private void SpawnRandomObjective()
    {
        int rndIndex = Random.Range(0, _levelConfiguration.Objectives.Count - 1);
        ObjectiveConfiguration config = _levelConfiguration.Objectives[rndIndex];
        ObjectiveManager.SpawnObjective(config);
        _nextSpawn = _currentRespawnTime;
    }
}
