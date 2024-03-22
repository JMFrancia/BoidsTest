using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/RandomBehavior")]
public class RandomBehavior : AbstractFlockBehavior
{
    [SerializeField] private float _agentSmoothTime = 0.5f;
    
    private Vector2 currentVelocity;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        return Vector2.SmoothDamp(
            agent.transform.up,
            new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)),
            ref currentVelocity,
            _agentSmoothTime
        );
    }
}
