using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    internal static LevelManager Instance;
    public LevelConfiguration CurrentLevel { get; private set; }
    [SerializeField]
    private List<LevelConfiguration> Levels;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        if (CurrentLevel == null)
            CurrentLevel = Levels[0];
    }

    internal void LoadNextLevel()
    {
        int currentIndex = Levels.IndexOf(CurrentLevel) + 1;
        LoadLevel(currentIndex);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Play Scene");
    }

    public void LoadLevel(int index)
    {
        if (index < 0 || index >= Levels.Count)
            return;

        CurrentLevel = Levels[index];
        SceneManager.LoadScene("Play Scene");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public bool HasNextLevel(LevelConfiguration config)
    {
        int index = Levels.IndexOf(config);
        if (index == -1)
            return false;

        ++index;
        return index < Levels.Count;
    }
}
