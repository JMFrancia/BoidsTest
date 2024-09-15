using System;
using System.Collections;
using UnityEngine;

/*
 * Extends FlockAgent to allow for conversion of human agents to zombie agents
 */
//TODO: Does this need to extend FlockAgent? 
public class ZombieFlockAgent : FlockAgent
{
    [SerializeField] private float _zombieConversionTime = 1.5f;
    [SerializeField] private FlockAgent _zombieAgentPrefab;

    private void OnEnable()
    {
        //TODO: Just use separate animator for zombies
        _animator.SetBool(Constants.AnimationBools.IS_ZOMBIE, true);
        _animator.SetTrigger(Constants.AnimationTriggers.RUN);
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherAgent = other.GetComponent<FlockAgent>();
        if (otherAgent == null)
            return;
        
        if (otherAgent.Flock.Faction.Equals(Constants.Factions.HUMANS))
        {
            StartCoroutine(ConvertToZombie(otherAgent));
        }
    }

    private IEnumerator ConvertToZombie(FlockAgent target)
    {
        Paused = true;
        target.Paused = true;
        yield return new WaitForSeconds(_zombieConversionTime);
        if(target != null)
            ReplaceWithZombie(target);
        Paused = false;
    }
    
    private void ReplaceWithZombie(FlockAgent target)
    {
        var zombie = Instantiate(_zombieAgentPrefab, target.transform.position, target.transform.rotation);
        target.Flock.RemoveFromFlock(target);
        Flock.AddToFlock(zombie);
        Destroy(target.gameObject);
    }
}
