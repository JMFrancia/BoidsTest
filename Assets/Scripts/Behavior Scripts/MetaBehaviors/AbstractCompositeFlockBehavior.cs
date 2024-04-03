/*
 * Abstract class for composite flock behaviors that are made up of multiple behaviors
 */
public abstract class AbstractCompositeFlockBehavior : AbstractFlockBehavior
{
    public abstract WeightedBehavior[] Behaviors { get;}
}