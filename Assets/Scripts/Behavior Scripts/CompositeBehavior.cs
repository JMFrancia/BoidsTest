using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : AbstractFlockBehavior
{
    public WeightedBehavior[] Behaviors => _behaviors;

    [SerializeField] private WeightedBehavior[] _behaviors;
    
    [Serializable] public struct WeightedBehavior
    {
        public AbstractFlockBehavior Behavior;
        public float Weight;
        
        public WeightedBehavior(AbstractFlockBehavior behavior, float weight)
        {
            Behavior = behavior;
            Weight = weight;
        }
    }

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        var move = Vector2.zero;
        foreach (var behavior in _behaviors)
        {
            if(behavior.Weight == 0) 
                continue;
            
            move += CalculateBehaviorMove(behavior, agent, context, flock);
        }
        return move;
    }
    
    private Vector2 CalculateBehaviorMove(WeightedBehavior weightedBehavior, FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (weightedBehavior.Behavior is CompositeBehavior behavior && behavior == this)
        {
            Debug.LogError("Circular reference detected in CompositeBehavior. Returning Vector2.zero");
            return Vector2.zero;
        }

        var move = weightedBehavior.Behavior.CalculateMove(agent, context, flock) * weightedBehavior.Weight;
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
