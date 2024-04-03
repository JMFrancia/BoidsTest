using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Flock/Behavior/SliderControlledLerpedCompositeBehavior")]
/*
 * Composite behavior that lerps between two behaviors based on the "aggression" slider value
 */
public class SliderControlledLerpedCompositeBehavior : LerpedCompositeBehavior
{
    private void OnEnable()
    {
        EventManager.StartListening(Constants.Events.SLIDER_AGGRESSION_CHANGED, (UnityAction<float>)OnAggressionChanged);
    }
    
    private void OnDisable()
    {
        EventManager.StopListening(Constants.Events.SLIDER_AGGRESSION_CHANGED, (UnityAction<float>)OnAggressionChanged);
    }
    
    private void OnAggressionChanged(float value)
    {
        LerpValue = value;
    }
}