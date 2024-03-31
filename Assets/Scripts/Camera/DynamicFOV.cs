using System;
using DG.Tweening;
using UnityEngine;

public class DynamicFOV : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _minFOV = 60f;
    [SerializeField] private float _maxFOV = 120f;
    [SerializeField] private float _zoomSpeed = 1f;

    private Tween _tween;

    private void OnEnable()
    {
        EventManager.StartListeningClass(Constants.Events.AGENT_ADDED_TO_FLOCK, OnFlockChanged);
    }

    private void OnDisable()
    {
        EventManager.StopListeningClass(Constants.Events.AGENT_ADDED_TO_FLOCK, OnFlockChanged);
    }
    
    private void OnFlockChanged(FlockAgent agent)
    {
        if(agent.Flock.Faction.Equals(Constants.Factions.ZOMBIES))
        {
            float t = (float)agent.Flock.Population / (float)FlockManager.Instance.WorldPopulation;
            SetFOV(t);
        }
    }

    private void Start()
    {
        _camera.fieldOfView = _minFOV;
    }

    public void SetFOV(float t)
    {
        if (_tween != null && _tween.active)
        {
            _tween.Kill();
        }

        var newFOV = Mathf.Lerp(_minFOV, _maxFOV, t);
        var deltaFOV = Mathf.Abs(_camera.fieldOfView - newFOV);
        var zoomTime = deltaFOV / _zoomSpeed;
        _tween = DOTween.To(() => _camera.fieldOfView, x => _camera.fieldOfView = x, newFOV, zoomTime).SetEase(Ease.InOutSine);
    }
}
