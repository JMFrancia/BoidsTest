using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/FireAtTarget")]
/*
 * Behavior that makes the agent fire at the nearest target
 */
public class FireAtTargetBehavior : AbstractFilteredFlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts contexts, Flock flock)
    {
        if(!agent.TryGetComponent<CanFireWeapon>(out var canFireWeapon))
            return Vector2.zero;

        var context = contexts.lineOfSightContext;
        var filteredContext = _filter.Filter(agent, context);
        if (filteredContext.Count == 0)
            return Vector2.zero;
        
        var nearestTarget = GetNearestTarget(agent, filteredContext);
        canFireWeapon.FireWeapon(nearestTarget.GetComponent<FlockAgent>());
        return Vector2.zero;
    }

    //TODO: This should be a helper function in a utility class
    private Transform GetNearestTarget(FlockAgent agent, List<Transform> context)
    {
        Transform nearestTarget = null;
        float minDistance = float.MaxValue;
        foreach (var item in context)
        {
            if (item == agent.transform)
                continue;
            
            var distance = Vector2.Distance(agent.transform.position, item.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = item;
            }
        }

        return nearestTarget;
    }
}
