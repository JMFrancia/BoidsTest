using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
/*
 * Behavior that aligns the agent with its neighbors
 */
public class AlignmentBehavior : AbstractFilteredFlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts contexts, Flock flock)
    {
        var context = contexts.neighborhoodContext;
        //if no neighbors, maintain current alignment
        if (context.Count == 0)
        {
            return agent.transform.up;
        }

        var alignmentMove = Vector2.zero;
        var filteredContext = _filter == null ? context : _filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            alignmentMove += (Vector2)item.transform.up;
        }
        alignmentMove /= context.Count;
        
        return alignmentMove;
    }
}
