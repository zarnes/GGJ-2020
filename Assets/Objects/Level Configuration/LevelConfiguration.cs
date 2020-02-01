using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Configuration", menuName = "Inventory System/Level Configuration", order = 0)]
public class LevelConfiguration : ScriptableObject
{
    public int LevelTime = 120;

    public List<StockItemConfiguration> ItemsInStock;
    public List<ObjectiveConfiguration> Objectives;
}
