using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Objective Data", menuName = "Inventory System/Objective", order = 0)]
public class ObjectiveConfiguration : ScriptableObject
{
    public GridObjectData Object;
    public GameObject ScrollInfos;
    public int Points = 100;
}
