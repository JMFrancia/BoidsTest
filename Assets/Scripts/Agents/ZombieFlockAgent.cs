using System.Collections;
using UnityEngine;

public class ZombieFlockAgent : FlockAgent
{
    [SerializeField] private float _zombieConversionTime = 1.5f;
    [SerializeField] private FlockAgent _zombieAgentPrefab;
    
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
