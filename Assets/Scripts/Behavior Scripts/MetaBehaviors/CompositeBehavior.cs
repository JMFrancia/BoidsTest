using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : AbstractCompositeFlockBehavior
{
    public override WeightedBehavior[] Behaviors => _behaviors;

    [SerializeField] private WeightedBehavior[] _behaviors;

    public override Vector2 CalculateMove(FlockAgent agent, Flock.Contexts contexts, Flock flock)
    {
        var move = Vector2.zero;
        foreach (var behavior in _behaviors)
        {
            if(behavior.Weight == 0) 
                continue;
            
            move += CalculateBehaviorMove(behavior, agent, contexts, flock);
        }
        return move;
    }
    
    private Vector2 CalculateBehaviorMove(WeightedBehavior weightedBehavior, FlockAgent agent, Flock.Contexts contexts, Flock flock)
    {
        //TODO: Search all child composites as well for circular references
        if (weightedBehavior.Behavior is CompositeBehavior behavior && behavior == this)
        {
            Debug.LogError($"Circular reference detected in CompositeBehavior {this}. Returning Vector2.zero");
            return Vector2.zero;
        }

        var move = weightedBehavior.Behavior.CalculateMove(agent, contexts, flock) * weightedBehavior.Weight;
        if (move != Vector2.zero)
        {
            if (move.sqrMagnitude > weightedBehavior.Weight * weightedBehavior.Weight)
            {
                move.Normalize();
                move *= weightedBehavior.Weight;
            }
        }
        return move;
    }
}
