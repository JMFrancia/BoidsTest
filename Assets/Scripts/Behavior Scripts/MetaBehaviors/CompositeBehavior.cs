using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Composite behavior that combines multiple behaviors into one
 */
[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : AbstractCompositeFlockBehavior
{
    public override WeightedBehavior[] Behaviors => _behaviors;

    [SerializeField] private WeightedBehavior[] _behaviors;

    private bool enabled;

    private void Awake()
    {
        enabled = !CheckCircularReferences(this);
        if (!enabled)
        {
            Debug.LogError($"Circular reference detected in CompositeBehavior {this}. Disabling CompositeBehavior");
        }
    }

    public override Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts contexts, Flock flock)
    {
        if(!enabled)
            return Vector2.zero;
        
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
        //TODO: Search all child composites as well for circular references. WILL HARD-CRASH UNITY OTHERWISE
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

    private bool CheckCircularReferences(AbstractCompositeFlockBehavior behavior, HashSet<AbstractCompositeFlockBehavior> visited = null)
    {
        if(visited == null)
            visited = new HashSet<AbstractCompositeFlockBehavior>();
        
        if(visited.Contains(behavior))
        {
            Debug.LogError($"Circular reference detected in CompositeBehavior {this}. Returning Vector2.zero");
            return true;
        }
        
        visited.Add(behavior);
        foreach(var weightedBehavior in behavior.Behaviors)
        {
            if(weightedBehavior.Behavior is AbstractCompositeFlockBehavior compositeBehavior)
            {
                if(CheckCircularReferences(compositeBehavior, visited))
                    return true;
            }
        }
        return false;
    }
}
