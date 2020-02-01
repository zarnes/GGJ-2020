using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public List<ObjectiveSlot> ObjectivesSlots;
    public GameObject ObjectiveFeedbackPrefab;
    
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

            slot.Configuration = configuration;
            slot.Occupied = true;
            GameObject feedbackGo = Instantiate(ObjectiveFeedbackPrefab);
            feedbackGo.GetComponent<ObjectiveFeedback>().Init(configuration);
            return true;
        }
        return false;
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
    internal bool Occupied;
    internal ObjectiveConfiguration Configuration;
    internal float TimeLeft;
}
