using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/SpecificFlock")]
public class SpecificFlockFilter : AbstractFlockFilter
{
    [SerializeField] private string _flockName;
    
    protected override string GetTargetFaction(FlockAgent agent) => _flockName;
}
