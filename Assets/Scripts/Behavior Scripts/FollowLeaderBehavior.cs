using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Flock/Behavior/FollowLeaderBehavior")]
/*
 * Behavior that makes the agent follow a "leader" gameobject with specified tag
 */
public class FollowLeaderBehavior : AbstractFlockBehavior
{
    //Agents will slowdown when they are within the radius of the leader
    [SerializeField] private float _slowdownFactor = 0.1f;

    //TODO: Use const list of tags
    [SerializeField] private string _leaderTag;
    [FormerlySerializedAs("_radius")] 
    [SerializeField] private float _slowdownRadius = 15f; //Radius for slowdown factor
    [Range(0f, 1f)]
    [SerializeField] private float _radiusThreshold = .9f; //If closer than this, don't follow leader this step
    
    [SerializeField] private bool _debugDrawRadius = true;

    private Transform Leader => GetLeader(_leaderTag);
    
    private Transform _leader;
    private static Dictionary<string, Transform> _leaderDict;

    //Getter function for leader from tag
    private Transform GetLeader(string leaderTag)
    {
        if (_leader != null)
            return _leader;
        
        if(_leaderDict == null)
            _leaderDict = new Dictionary<string, Transform>();
        else if (_leaderDict.ContainsKey(leaderTag))
            return _leaderDict[leaderTag];
        
        var leader = GameObject.FindGameObjectWithTag(leaderTag)?.transform;
        if (leader == null)
        {
            Debug.LogError($"No leader found with tag {leaderTag}");
            return null;
        }

        _leader = leader;
        return leader;
    }

    public override Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts context, Flock flock)
    {
        if (Leader == null)
        {
            Debug.Log("No leader found! Returning Vector2.zero");
            return Vector2.zero;
        }

        var centerOffset = (Vector2)Leader.position - (Vector2)agent.transform.position;  // offset from the leader
        var t = centerOffset.magnitude / _slowdownRadius; // if t > 1, agent is outside the radius
        agent.VelocityMultiplier = Mathf.Lerp(_slowdownFactor, 1f, t); // slow down if inside the radius
        if (t < _radiusThreshold) // if t is within radius threshold, do nothing
        {
            return Vector2.zero;
        }

        var result = centerOffset * (t * t);
        return result;
    }

    //Does this even work?
    private void OnDrawGizmos()
    {
        if (!_debugDrawRadius || Leader == null)
            return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Leader.position, _slowdownRadius);
    }
}
