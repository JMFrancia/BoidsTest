using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/SameFlock")]
public class SameFlockFilter : AbstractFlockFilter
{
    protected override string GetTargetFaction(FlockAgent agent) => agent.Flock.Faction; 
}
