using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/SteeredAlignment")]
public class SteeredAlignmentBehavior : AlignmentBehavior
{
    [SerializeField] private float _agentSmoothTime = 0.5f;
    
    private Vector2 currentVelocity;
    
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        return Vector2.SmoothDamp(
            agent.transform.up,
            base.CalculateMove(agent, context, flock),
            ref currentVelocity,
            _agentSmoothTime
            );
    }
}
