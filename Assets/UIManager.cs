using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] LoadingBarController _zombieLoadingBar;

    private void OnEnable()
    {
        EventManager.StartListeningClass(Constants.Events.AGENT_ADDED_TO_FLOCK, OnFlockChanged);
        EventManager.StartListeningClass(Constants.Events.AGENT_DIED, OnFlockAgentDied);
    }
    
    private void OnDisable()
    {
        EventManager.StopListeningClass(Constants.Events.AGENT_ADDED_TO_FLOCK, OnFlockChanged);
        EventManager.StopListeningClass(Constants.Events.AGENT_DIED, OnFlockAgentDied);
    }

    private void Start()
    {
        var t = 1 / (float)FlockManager.Instance.WorldPopulation;
        _zombieLoadingBar.SetFillAmount(t);
    }

    private void OnFlockChanged(FlockAgent agent)
    {
        if(agent.Flock.FlockName == "zombies")
        {
            var t = (float)agent.Flock.Population / (float)FlockManager.Instance.WorldPopulation;
            _zombieLoadingBar.AnimateFillAmount(t);
        }
    }

    private void OnFlockAgentDied(FlockAgent agent)
    {
        if(agent.Flock.FlockName == "zombies")
        {
            var t = (float)agent.Flock.Population / (float)FlockManager.Instance.WorldPopulation;
            _zombieLoadingBar.SetFillAmount(t);
        }
    }
}
