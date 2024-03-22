using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractContextFilter : ScriptableObject
{
    public abstract List<Transform> Filter(FlockAgent agent, List<Transform> original);
}