using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SliderControlledRadiusFlock : Flock
{
    [SerializeField] private float _minAvoidenceRadiusMultiplier = .5f;
    [SerializeField] private float _maxAvoidenceRadiusMultiplier = 3f;

    private void OnEnable()
    {
        EventManager.StartListening(Constants.Events.SLIDER_SPREAD_CHANGED, (UnityAction<float>) OnSpreadSliderChanged);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Constants.Events.SLIDER_SPREAD_CHANGED, (UnityAction<float>) OnSpreadSliderChanged);
    }
    
    private void OnSpreadSliderChanged(float value)
    {
        _avoidanceRadiusMultiplier = Mathf.Lerp(_minAvoidenceRadiusMultiplier, _maxAvoidenceRadiusMultiplier, value);
        UpdateSquareAvoidanceRadius();
    }
}
