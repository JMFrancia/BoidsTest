using System.Collections;
using UnityEngine;

public class ZombieFlockAgent : FlockAgent
{
    [SerializeField] private float _zombieConversionTime = 1.5f;
    
    private void OnTriggerEnter(Collider other)
    {
        var otherAgent = other.GetComponent<FlockAgent>();
        if (otherAgent == null)
            return;
        
        if (otherAgent.Flock.FlockName == "humans")
        {
            StartCoroutine(ConvertToZombie(otherAgent));
        }
    }

    private IEnumerator ConvertToZombie(FlockAgent target)
    {
        Paused = true;
        target.Paused = true;
        yield return new WaitForSeconds(_zombieConversionTime);
        Flock.AddToFlock(target);
        target.GetComponentInChildren<SpriteRenderer>().color = Color.green;
        Paused = false;
        target.Paused = false;
    }
}
