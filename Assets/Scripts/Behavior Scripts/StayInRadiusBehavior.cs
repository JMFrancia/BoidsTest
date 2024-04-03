using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/StayInRadius")]
/*
 * Behavior that compels the agent stay in a certain radius of a given vector2
 */
public class StayInRadiusBehavior : AbstractFlockBehavior
{
    [SerializeField] private Vector2 _center;
    [SerializeField] private float _radius = 15f;
    [Range(0f, 1f)]
    [SerializeField] private float _radiusThreshold = .9f;
    
    [SerializeField] private bool _debugDrawRadius = true;
    
    public override Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts contexts, Flock flock)
    {
        var centerOffset = (Vector2)_center - (Vector2)agent.transform.position;
        var t = centerOffset.magnitude / _radius;
        if (t < _radiusThreshold)
        {
            return Vector2.zero;
        }

        return centerOffset * (t * t);
    }

    private void OnDrawGizmos()
    {
        if (!_debugDrawRadius)
            return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_center, _radius);
    }
}
