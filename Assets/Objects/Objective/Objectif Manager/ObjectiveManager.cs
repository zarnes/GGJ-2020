﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public List<ObjectiveSlot> ObjectivesSlots;
    public GameObject ObjectiveFeedbackPrefab;

    public int ActiveObjectives { get; private set; }

    public void LoadConfiguration(LevelConfiguration configuration)
    {
        // Spawn tutorial objective
        SpawnObjective(configuration.Objectives[0]);
    }
    
    public bool SpawnObjective(ObjectiveConfiguration configuration)
    {
        foreach(ObjectiveSlot slot in ObjectivesSlots)
        {
            if (slot.Occupied)
                continue;
            
            GameObject feedbackGo = Instantiate(ObjectiveFeedbackPrefab, transform);
            ObjectiveFeedback of = feedbackGo.GetComponent<ObjectiveFeedback>();
            of.Init(configuration);
            feedbackGo.transform.position = transform.position + new Vector3(slot.Position.x, slot.Position.y);

            slot.Configuration = configuration;
            slot.Occupied = true;
            slot.Feedback = of;

            ++ActiveObjectives;

            return true;
        }
        return false;
    }

    public int ValidateObjectives(GridInventory outputInventory)
    {
        int points = 0;
        int objectivesCompleted = 0;
        foreach(ObjectiveSlot slot in ObjectivesSlots)
        {
            if (!slot.Occupied)
                continue;

            GridObject gObj = outputInventory.GetObjectWithData(slot.Configuration.Object);
            if (gObj != null)
            {
                Debug.Log("Completed objective for item " + gObj.name);
                outputInventory.RemoveObject(gObj, true);

                points += slot.Configuration.Points;
                ++objectivesCompleted;

                slot.Feedback.Complete();
                slot.Occupied = false;
                slot.Configuration = null;
                slot.Feedback = null;

                --ActiveObjectives;
            }
        }

        switch(objectivesCompleted)
        {
            case 2:
                points = (int)((float)points * 1.15f);
                break;
            case 3:
                points = (int)((float)points * 1.5f);
                break;
            case 4:
                points = (int)((float)points * 2f);
                break;
        }

        points -= outputInventory.FlushItems() * 10;
        return points;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        foreach(ObjectiveSlot objective in ObjectivesSlots)
        {
            Vector3 position = transform.position;
            position += new Vector3(objective.Position.x, objective.Position.y);
            Gizmos.DrawWireSphere(position, .5f);
        }
    }
}

[System.Serializable]
public class ObjectiveSlot
{
    public Vector2 Position;
    [ReadOnly]
    public bool Occupied;
    [ReadOnly]
    public ObjectiveConfiguration Configuration;
    
    internal ObjectiveFeedback Feedback;
}
