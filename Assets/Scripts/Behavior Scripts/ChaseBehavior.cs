using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Chase")]
public class ChaseBehavior : AvoidenceBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, Flock.Contexts contexts, Flock flock)
    {
        return base.CalculateMove(agent, contexts, flock) * -1f;
    }
}