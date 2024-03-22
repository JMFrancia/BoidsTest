using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidence")]
public class AvoidenceBehavior : AbstractFilteredFlockBehavior
{
    [SerializeField] private float _speedUpFactor = 3f;
    
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
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
            if (dist < flock.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position - item.position);
            }
        }

        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }
        
        agent.VelocityMultiplier = Mathf.Lerp(1f, _speedUpFactor, 1f - (closestDist / flock.SquareAvoidanceRadius));

        return avoidanceMove;
    }
}
