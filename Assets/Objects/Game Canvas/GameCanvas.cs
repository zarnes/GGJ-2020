using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public ObjectiveManager ObjectiveManager;
    public GridInventory OutputInventory;

    public void ValidateOutput()
    {
        ObjectiveManager.ValidateObjectives(OutputInventory);
    }
}
