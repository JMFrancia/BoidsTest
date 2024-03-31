using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFlockFilter : AbstractContextFilter
{
    protected static Dictionary<Transform, Flock> _flockAgents = new ();

    //TODO: Should be flock. Some singleton manager to grab flock from name perhaps?
    //ALSO: Constant flock names
    protected abstract string GetTargetFaction(FlockAgent agent);
    
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        var filtered = new List<Transform>();
        foreach (var item in original)
        {
            Flock itemFlock;
            if (!_flockAgents.ContainsKey(item))
            {
                _flockAgents[item] = item.GetComponent<FlockAgent>()?.Flock;
            }
            itemFlock = _flockAgents[item];
            if(itemFlock == null)
                continue;
            if(itemFlock.Faction == GetTargetFaction(agent))
                filtered.Add(item);
        }
        return filtered;
    }
    
    private void OnEnable()
    {
        EventManager.StartListeningClass(Constants.Events.AGENT_ADDED_TO_FLOCK, OnFlockChanged);
    }

    private void OnDisable()
    {
        EventManager.StopListeningClass(Constants.Events.AGENT_ADDED_TO_FLOCK, OnFlockChanged);
    }
    
    private void OnFlockChanged(GameObject agent)
    {
        if (_flockAgents.ContainsKey(agent.transform))
        {
            _flockAgents.Remove(agent.transform);
        }
    }
}