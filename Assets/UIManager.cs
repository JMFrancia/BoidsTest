using System;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] LoadingBarController _zombieLoadingBar;
    [SerializeField] private TextMeshProUGUI _winText;

    private void OnEnable()
    {
        EventManager.StartListeningClass(Constants.Events.AGENT_ADDED_TO_FLOCK, OnFlockChanged);
        EventManager.StartListeningClass(Constants.Events.AGENT_DIED, OnFlockAgentDied);
        EventManager.StartListening(Constants.Events.FACTION_DEPLETED, OnFactionDepleted);
    }
    
    private void OnDisable()
    {
        EventManager.StopListeningClass(Constants.Events.AGENT_ADDED_TO_FLOCK, OnFlockChanged);
        EventManager.StopListeningClass(Constants.Events.AGENT_DIED, OnFlockAgentDied);
        EventManager.StopListening(Constants.Events.FACTION_DEPLETED, OnFactionDepleted);
    }

    private void Start()
    {
        var t = 1 / (float)FlockManager.Instance.WorldPopulation;
        _zombieLoadingBar.SetFillAmount(t);
        _winText.alpha = 0f;
    }

    private void OnFlockChanged(FlockAgent agent)
    {
        if(agent.Flock.Faction.Equals(Constants.Factions.ZOMBIES))
        {
            var t = (float)agent.Flock.Population / (float)FlockManager.Instance.WorldPopulation;
            _zombieLoadingBar.AnimateFillAmount(t);
        }
    }

    private void OnFlockAgentDied(FlockAgent agent)
    {
        if(agent.Flock.Faction.Equals(Constants.Factions.ZOMBIES))
        {
            var t = (float)agent.Flock.Population / (float)FlockManager.Instance.WorldPopulation;
            _zombieLoadingBar.SetFillAmount(t);
        }
    }
    
    private void OnFactionDepleted(string faction)
    {
        _winText.text = faction.Equals(Constants.Factions.HUMANS) ? "Zombies Win!" : "Humans Win!";
        _winText.color = faction.Equals(Constants.Factions.HUMANS) ? Color.green : new Color(255, 165, 0);
        _winText.alpha = 1f;
    }
}
