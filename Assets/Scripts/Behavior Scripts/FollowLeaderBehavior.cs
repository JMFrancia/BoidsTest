using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/FollowLeaderBehavior")]
public class FollowLeaderBehavior : AbstractFlockBehavior
{
    [SerializeField] private float _slowdownFactor = 0.1f;

    //TODO: Better way to do this than relying on a tag? If so, make tag from list of constants
    //Maybe have singleton manager that fetches meta data for the flock? Or flock keeps track of leader?
    [SerializeField] private string _leaderTag;
    [SerializeField] private float _radius = 15f;
    [Range(0f, 1f)]
    [SerializeField] private float _radiusThreshold = .9f;
    
    [SerializeField] private bool _debugDrawRadius = true;

    private Transform Leader
    {
        get
        {
            if(_leader == null)
                _leader = GameObject.FindGameObjectWithTag(_leaderTag)?.transform;
            return _leader;
        }
    }
    
    private Transform _leader;
    
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (Leader == null)
        {
            Debug.Log("No leader found! Returning Vector2.zero");
            return Vector2.zero;
        }

        var centerOffset = (Vector2)Leader.position - (Vector2)agent.transform.position;  // offset from the leader
        var t = centerOffset.magnitude / _radius; // if t > 1, agent is outside the radius
        agent.VelocityMultiplier = Mathf.Lerp(_slowdownFactor, 1f, t); // slow down if inside the radius
        if (t < _radiusThreshold) // if t is within radius threshold, do nothing
        {
            return Vector2.zero;
        }

        var result = centerOffset * (t * t);
        return result;
    }

    private void OnDrawGizmos()
    {
        if (!_debugDrawRadius || Leader == null)
            return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Leader.position, _radius);
    }
}
