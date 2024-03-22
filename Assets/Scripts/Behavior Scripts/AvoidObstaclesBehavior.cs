using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/AvoidObstacles")]
public class AvoidObstaclesBehavior : AbstractFilteredFlockBehavior
{
    private static Dictionary<Transform, Collider> _colliders = new Dictionary<Transform, Collider>();

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors, return no adjustment
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        //add all points together and average
        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;
        var filteredContext = _filter == null ? context : _filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            if (!_colliders.ContainsKey(item))
                _colliders[item] = item.GetComponent<Collider>();
            
            var collider = _colliders[item];
            Vector3 closestPoint = collider == null ? item.position : collider.ClosestPoint(agent.transform.position);
            if (Vector2.SqrMagnitude(closestPoint - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                nAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position - closestPoint);
            }
        }
        if (nAvoid > 0)
            avoidanceMove /= nAvoid;
        
        return avoidanceMove;
    }
}
