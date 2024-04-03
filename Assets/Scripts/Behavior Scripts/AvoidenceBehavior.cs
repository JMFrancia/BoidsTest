using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidence")]
/*
 * Behavior that moves the agent away from its neighbors
 */
public class AvoidenceBehavior : AbstractFilteredFlockBehavior
{
    //TODO: Create a standardized way to set which context to use for each behavior (maybe a string -> List dict for each context? or use int for key w/ enum?)
    [FormerlySerializedAs("_avoidLineOfSight")] 
    [SerializeField] private bool _useLineOfSight = false;
    [SerializeField] private float _speedUpFactor = 3f; //Speed up factor will multiply an agents velocity by this factor when avoiding
    
    public override Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts contexts, Flock flock)
    {
        var context = _useLineOfSight ? contexts.lineOfSightContext : contexts.immediateContext;
        //if no neighbors, return no adjustment
        if (context.Count == 0)
        {
            return Vector2.zero;
        }
        

        var filteredContext = _filter == null ? context : _filter.Filter(agent, context);
        if(filteredContext.Count == 0)
            return Vector2.zero;
        
        

        var avoidanceMove = Vector2.zero;
        int nAvoid = 0; //number of objects in avoidance radius
        float closestDist = Mathf.Infinity;
        foreach (Transform item in filteredContext)
        {
            var dist = Vector2.SqrMagnitude(item.position - agent.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
            }
            nAvoid++;
            avoidanceMove += (Vector2)(agent.transform.position - item.position);
        }

        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }
        
        agent.VelocityMultiplier = Mathf.Lerp(1f, _speedUpFactor, 1f - (closestDist / (_useLineOfSight ? flock.SquareLineOfSightRadius : flock.SquareAvoidanceRadius)));

        return avoidanceMove;
    }
}