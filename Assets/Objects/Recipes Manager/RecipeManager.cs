using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    internal static RecipeManager Instance;

    public List<Recipe> Recipes;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool ApplyRecipe(GridObject dragged, GridObject target)
    {
        Recipe validRecipe = GetRecipeWithInputs(dragged, target);
        if (validRecipe == null)
            return false;

        foreach(RecipeOutput output in validRecipe.Outputs)
        {
            GridObjectData targetInput = validRecipe.Inputs[output.InputObjectIndex].Object;

            GridObject inputObject = null;
            if (output.InputObjectIndex == 0)
                inputObject = dragged;
            else if (output.InputObjectIndex == 1)
                inputObject = target;

            if (inputObject == null)
            {
                Debug.LogError("Error in object index configuration for output (" + output.InputObjectIndex + ")", validRecipe);
                return false;
            }
            
            // TODO add relative position
            ObjectFactory.Instance.GenerateObject(inputObject.transform.position, output.Object, true);
        }

        int index = 0;
        bool placedBackObj = false;
        foreach(RecipeInput input in validRecipe.Inputs)
        {
            GridObject gObj = null;
            if (index == 0)
                gObj = dragged;
            else if (index == 1)
                gObj = target;
            else
                Debug.LogWarning("Trying to assign an object with invalid index in recipe (" + index + ")");

            if (input.Behavior == RecipeInput.CraftBehavior.Destroy)
            {
                GridSystem grid;
                Vector2Int gCoords;
                GridManager.Instance.GetGridCoords(gObj.transform.position, out grid, out gCoords);
                grid.Inventory.RemoveObject(gObj, true);
            }
            else if (input.Behavior == RecipeInput.CraftBehavior.PlaceBack)
            {
                gObj.transform.position = gObj.initialDragPosition;
                placedBackObj = true;
            }
            ++index;
        }

        return !placedBackObj;
    }

    private Recipe GetRecipeWithInputs(GridObject dragged, GridObject target)
    {
        List<Recipe> validRecipes = new List<Recipe>();
        foreach (Recipe recipe in Recipes)
        {
            bool aFound = false;
            bool bFound = false;

            foreach (RecipeInput input in recipe.Inputs)
            {
                if (input.Object == dragged.Data && !aFound)
                    aFound = true;
                else if (input.Object == target.Data && input.CanBeTargetObject)
                    bFound = true;
            }

            if (aFound && bFound)
                return recipe;
        }

        return null;
    }
}
