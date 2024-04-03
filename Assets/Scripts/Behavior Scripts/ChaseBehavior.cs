using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Chase")]
/*
 * Behavior that makes the agent chase other agents. Literally just Avoidence behavior reversed
 */
public class ChaseBehavior : AvoidenceBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts contexts, Flock flock)
    {
        return base.CalculateMove(agent, contexts, flock) * -1f;
    }
}