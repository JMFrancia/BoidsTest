using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/LerpedComposite")]
/*
 * Composite behavior that lerps between two behaviors
 */
public class LerpedCompositeBehavior : AbstractCompositeFlockBehavior
{
    public override WeightedBehavior[] Behaviors => new[] {_behavior1, _behavior2};
    
    public float LerpValue
    {
        get => _lerpValue;
        set => _lerpValue = Mathf.Clamp01(value);
    }
    
    [SerializeField] private WeightedBehavior _behavior1;
    [SerializeField] private WeightedBehavior _behavior2;
    [Range(0, 1)]
    [SerializeField] private float _defaultLerpValue = 0.5f;
    
    private float _lerpValue;

    protected void OnEnable()
    {
        _lerpValue = _defaultLerpValue;\
    }

    public override Vector2 CalculateMove(FlockAgent agent, in Flock.Contexts contexts, Flock flock)
    {
        var move = Vector2.zero;
        if(_lerpValue == 0) 
            move = _behavior1.Behavior.CalculateMove(agent, contexts, flock);
        else if (_lerpValue == 1) 
            move = _behavior2.Behavior.CalculateMove(agent, contexts, flock);
        else
        {
            move = _behavior1.Behavior.CalculateMove(agent, contexts, flock) * (1 - _lerpValue) +
                   _behavior2.Behavior.CalculateMove(agent, contexts, flock) * _lerpValue;
        }
        
        return move;
    }
}