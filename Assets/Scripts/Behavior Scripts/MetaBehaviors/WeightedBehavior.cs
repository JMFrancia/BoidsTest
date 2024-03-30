using System;

[Serializable] public struct WeightedBehavior
{
    public AbstractFlockBehavior Behavior;
    public float Weight;
        
    public WeightedBehavior(AbstractFlockBehavior behavior, float weight)
    {
        Behavior = behavior;
        Weight = weight;
    }
}