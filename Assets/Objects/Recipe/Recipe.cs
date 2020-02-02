using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe Data", menuName = "Inventory System/Recipe", order = 0)]
public class Recipe : ScriptableObject
{
    public List<RecipeInput> Inputs;
    public List<RecipeOutput> Outputs;

    [Space]
    public MusicManager.SoundConfig Sound;

    [Space]
    public GridObjectData Accident;
    public float AccidentPropability = .5f;
}
