using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/SameFlock")]
public class SameFlockFilter : AbstractFlockFilter
{
    protected override string GetTargetFlockName(FlockAgent agent) => agent.Flock.FlockName; 
}
