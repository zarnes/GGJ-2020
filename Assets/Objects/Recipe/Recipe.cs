using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe Data", menuName = "Inventory System/Recipe", order = 0)]
public class Recipe : ScriptableObject
{
    public List<RecipeInput> Inputs;
    public List<RecipeOutput> Outputs;
}
