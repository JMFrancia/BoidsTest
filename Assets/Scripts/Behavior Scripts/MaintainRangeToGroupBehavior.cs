using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Flock/Behavior/MaintainRangeToGroup")]
public class MaintainRangeToGroupBehavior : AbstractFilteredFlockBehavior
{
    [SerializeField] private float _minDistance = 5f;
    [SerializeField] private float _maxDistance = 10f;
    
    public override Vector2 CalculateMove(FlockAgent agent, Flock.Contexts contexts, Flock flock)
    {
        var context = contexts.lineOfSightContext;
        //if no neighbors, return no adjustment
        if (context.Count == 0)
        {
            return Vector2.zero;
        }

        var filteredContext = _filter == null ? context : _filter.Filter(agent, context);
        if(filteredContext.Count == 0)
            return Vector2.zero;
        
        var chaseMove = Vector2.zero;
        int nChase = 0; //number of objects in LOS radius
        float closestDist = Mathf.Infinity;
        foreach (Transform item in filteredContext)
        {
            var dist = Vector2.SqrMagnitude(item.position - agent.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
            }
            nChase++;
            chaseMove += (Vector2)(item.position - agent.transform.position);
        }

        if (nChase > 0)
        {
            chaseMove /= nChase;
        }

        Debug.Log("Closest dist: " + closestDist);
        if(closestDist < _minDistance) // if too close, move away
        {
            Debug.Log("Moving away from group!");
            return -1 * chaseMove;
        }
        else if(closestDist > _maxDistance) // if too far, move closer
        {
            Debug.Log("Moving toward from group!");
            return chaseMove;
        }
        else
        {
            Debug.Log("staying in range to group!");
            return Vector2.zero;
        }
    }
}