using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/TimeControlledLerpedCompositeBehavior")]
/*
 * Composite behavior that lerps between two behaviors for a limited period of time when triggered
 */
public class TimeControlledLerpedCompositeBehavior : LerpedCompositeBehavior
{
    //TODO: Store this duration somewhere else
    [SerializeField] private float _duration = 5f;
    [SerializeField] private bool _interuptable;
    
    private bool _activated = false;
    private Sequence _sequence;

    private void OnEnable()
    {
        _activated = false;
        base.OnEnable();
        EventManager.StartListening(Constants.Events.FRENZY_ACTIVATED, OnFrenzyActivated);
    }
    
    private void OnDisable()
    {
        EventManager.StopListening(Constants.Events.FRENZY_ACTIVATED, OnFrenzyActivated);
    }
    
    private void OnFrenzyActivated()
    {
        Activate();
    }
    
    private void Activate()
    {
        if (_activated)
        {
            if (!_interuptable)
                return;

            _sequence.Kill();
        }

        LerpValue = 1f;
        _activated = true;
        _sequence = DOTween.Sequence();
        _sequence.AppendInterval(_duration);
        _sequence.OnComplete(Deactivate);
        _sequence.Play();
    }

    private void Deactivate()
    {
        LerpValue = 0f;
        _activated = false;
    }
}