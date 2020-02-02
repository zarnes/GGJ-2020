using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal static GameManager Instance;

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

    [SerializeField]
    private GridSystem InputGridSystem;

    [SerializeField]
    private Transform _tutoPanel;

    private int _points;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        _levelConfiguration = LevelManager.Instance.CurrentLevel;

        _currentRespawnTime = _levelConfiguration.RespawnTime;
        _nextSpawn = _currentRespawnTime;
        ObjectiveManager.LoadConfiguration(_levelConfiguration);

        if (!_levelConfiguration.AllowFirstInfiniteRecipe)
            _firstObjectiveFinished = true;

        _tutoPanel.gameObject.SetActive(!_firstObjectiveFinished);

        ObjectFactory objFactory = ObjectFactory.Instance;
        foreach (StockItemConfiguration itemConfiguration in _levelConfiguration.ItemsInStock)
            objFactory.GenerateObject(InputGridSystem, itemConfiguration.Position, itemConfiguration.Object);

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
            {
                _tutoPanel.gameObject.SetActive(false);
                _firstObjectiveFinished = true;
            }

            SpawnRandomObjective();
        }
        else if (_nextSpawn <= 0)
        {
            SpawnRandomObjective();
        }

        if (Input.GetKeyDown(KeyCode.R))
            LevelManager.Instance.ReloadLevel();

        if (Input.GetKeyDown(KeyCode.Escape))
            LevelManager.Instance.LoadMenu();

        if (TimeRemaining <= 0)
        {
            Debug.Log("TIME OUT");
            gameObject.SetActive(false);
            GameCanvas.EndGame(_points, LevelManager.Instance.HasNextLevel(_levelConfiguration));
        }
    }

    private void SpawnRandomObjective()
    {
        int rndIndex = Random.Range(0, _levelConfiguration.Objectives.Count);
        ObjectiveConfiguration config = _levelConfiguration.Objectives[rndIndex];
        ObjectiveManager.SpawnObjective(config);

        /*ObjectFactory objFactory = ObjectFactory.Instance;
        foreach (StockItemConfiguration itemConfiguration in _levelConfiguration.ItemsInStock)
            objFactory.GenerateObject(InputGridSystem, itemConfiguration.Position, itemConfiguration.Object);*/
        
        _nextSpawn = _currentRespawnTime;
    }

    public void AddPoints(int points)
    {
        _points += points;
        GameCanvas.Instance.SetScore(_points);
    }
}
