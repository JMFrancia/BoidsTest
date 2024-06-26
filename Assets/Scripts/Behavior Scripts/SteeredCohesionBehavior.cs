using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/SteeredCohesion")]
/*
 * Behavior that makes the agent move towards the average position of its neighbors (steering smooths it out)
 */
public class SteeredCohesionBehavior : CohesionBehavior
{
    [SerializeField] private float _agentSmoothTime = 0.5f;
    
    private Vector2 currentVelocity;
    
    public override Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts contexts, Flock flock)
    {
        return Vector2.SmoothDamp(
            agent.transform.up,
            base.CalculateMove(agent, contexts, flock),
            ref currentVelocity,
            _agentSmoothTime
            );
    }
}