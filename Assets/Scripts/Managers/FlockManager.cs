using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlockManager : MonoBehaviour
{
    public int WorldPopulation => GetWorldPopulation();
    
    public static FlockManager Instance => _instance;
        
    private static FlockManager _instance;

    private Flock[] _flocks;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _flocks = GetComponentsInChildren<Flock>();
    }

    private void OnEnable()
    {
        EventManager.StartListeningClass(Constants.Events.FLOCK_DEPLETED, OnFlockDepleted);
    }

    private void OnDisable()
    {
        EventManager.StopListeningClass(Constants.Events.FLOCK_DEPLETED, OnFlockDepleted);
    }
    
    private void OnFlockDepleted(Flock flock)
    {
        if (GetFactionPopulation(flock.Faction) == 0)
        {
            EventManager.TriggerEvent(Constants.Events.FACTION_DEPLETED, flock.Faction);
        }
    }

    private int GetWorldPopulation()
    {
        int total = 0;
        for(int n = 0; n < _flocks.Length; n++)
        {
            total += _flocks[n].Population;
        }
        return total;
    }
    
    private int GetFactionPopulation(string faction)
    {
        int total = 0;
        for(int n = 0; n < _flocks.Length; n++)
        {
            if(_flocks[n].Faction.Equals(faction))
                total += _flocks[n].Population;
        }
        return total;
    }
}
