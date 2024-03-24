using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/FireAtTarget")]
public class FireAtTargetBehavior : AbstractFilteredFlockBehavior
{
    [SerializeField] float _reloadTime = 1.5f;

    private bool _reloading;
    private float _timeSinceLastShot;

    public override Vector2 CalculateMove(FlockAgent agent, Flock.Contexts contexts, Flock flock)
    {
        UpdateReload();
        if(_reloading || !agent.TryGetComponent<CanFireWeapon>(out var canFireWeapon))
            return Vector2.zero;

        var context = contexts.lineOfSightContext;
        var filteredContext = _filter.Filter(agent, context);
        if (filteredContext.Count == 0)
            return Vector2.zero;
        
        var nearestTarget = GetNearestTarget(agent, filteredContext);
        canFireWeapon.FireWeapon(nearestTarget.GetComponent<FlockAgent>());
        _reloading = true;
        _timeSinceLastShot = 0f;
        
        return Vector2.zero;
    }

    private void UpdateReload()
    {
        if (_reloading)
        {
            _timeSinceLastShot += Time.deltaTime;
            if(_timeSinceLastShot >= _reloadTime)
                _reloading = false;
        }
    }

    private Transform GetNearestTarget(FlockAgent agent, List<Transform> context)
    {
        Transform nearestTarget = null;
        float minDistance = float.MaxValue;
        foreach (var item in context)
        {
            if (item == agent.transform)
                continue;
            
            var distance = Vector2.Distance(agent.transform.position, item.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTarget = item;
            }
        }

        return nearestTarget;
    }
}
