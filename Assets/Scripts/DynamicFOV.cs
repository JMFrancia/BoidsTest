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
        EventManager.StartListeningClass(Constants.Events.FLOCK_CHANGED, OnFlockChanged);
    }

    private void OnDisable()
    {
        EventManager.StopListeningClass(Constants.Events.FLOCK_CHANGED, OnFlockChanged);
    }
    
    private void OnFlockChanged(GameObject agent)
    {
        if(agent.TryGetComponent<FlockAgent>(out var flockAgent) && flockAgent.Flock.FlockName == "zombies")
        {
            float t = (float)flockAgent.Flock.Population / (float)FlockManager.Instance.WorldPopulation;
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
        Debug.Log("Setting FOV to " + newFOV + " in " + zoomTime + " seconds");
        _tween = DOTween.To(() => _camera.fieldOfView, x => _camera.fieldOfView = x, newFOV, zoomTime).SetEase(Ease.InOutSine);
    }
}