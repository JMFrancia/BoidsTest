using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Cohesion")]
/*
 * Behavior that makes the agent move towards the center of its neighbors
 */
public class CohesionBehavior : AbstractFilteredFlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts contexts, Flock flock)
    {
        var context = contexts.neighborhoodContext;
        //if no neighbors, return no adjustment
        if (context.Count == 0)
        {
            return Vector2.zero;
        }
        
        //If no neighbors after filter, return no adjustment
        var filteredContext = _filter == null ? context : _filter.Filter(agent, context);
        if (filteredContext.Count == 0)
            return Vector2.zero;
        
        //add all points together and average
        //TODO: Multiple behaviors get average of several points. Should this be refactored into a helper function?
        Vector2 cohesionMove = Vector2.zero;
        if (filteredContext.Count == 1)
            cohesionMove = filteredContext[0].position;
        else
        {
            foreach (Transform item in filteredContext)
            {
                cohesionMove += (Vector2)item.position;
            }
            cohesionMove /= context.Count;
        }
        
        //create offset from agent position
        cohesionMove -= (Vector2)agent.transform.position;
        
        return cohesionMove;
    }
}
