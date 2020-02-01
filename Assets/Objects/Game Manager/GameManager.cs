using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelConfiguration LevelConfiguration;

    [Space]
    public ObjectiveManager ObjectiveManager;

    // Start is called before the first frame update
    void Start()
    {
        ObjectiveManager.LoadConfiguration(LevelConfiguration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
