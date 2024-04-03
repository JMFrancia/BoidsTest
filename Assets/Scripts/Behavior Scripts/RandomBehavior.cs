using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/RandomBehavior")]
/*
 * Behavior that makes the agent move in a random direction
 */
public class RandomBehavior : AbstractFlockBehavior
{
    [SerializeField] private float _agentSmoothTime = 0.5f;
    
    private Vector2 currentVelocity;

    public override Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts contexts, Flock flock)
    {
        return Vector2.SmoothDamp(
            agent.transform.up,
            new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)),
            ref currentVelocity,
            _agentSmoothTime
        );
    }
}
