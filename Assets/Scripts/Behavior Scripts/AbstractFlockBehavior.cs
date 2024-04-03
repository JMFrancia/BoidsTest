using UnityEngine;

public abstract class AbstractFlockBehavior : ScriptableObject
{
    //TODO: Come up with a better way to do this than passing in the whole context struct every time.There must be some better way to tell behaviors which context to use.
    public abstract Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts contexts, Flock flock);
}
