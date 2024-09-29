using System;
using UnityEditor;
using UnityEngine;

/* 
    Controls a flock agent
*/
[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    public Collider AgentCollider => _agentCollider;
    public bool Paused
    {
        get => _paused;
        set => _paused = value;
    }

    [SerializeField] private float _turnSmoothTime = 10f; //Smooths turns
    [SerializeField] private float _moveThreshold = 0.5f; //Minimum distance to move before updating position
    [SerializeField] private float _animationStateChangeCooldown = .2f;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] protected Animator _animator;
    
    protected AnimationState _animationState;
    
    private Vector2 _oldPosition;
    private bool _paused;
    private bool _canChangeAnimationState;
    private float _timePassedSinceAnimationStateChange;
    
    public enum AnimationState
    {
        Running,
        Attacking,
        Idle,
        Dying,
        Dead
    }

    public Flock Flock
    {
        get => _flock;
        set => _flock = value;
    }
    
    //Used to modify speed for certain behaviors. Should have a better system for increasing / decreasing this
    public float VelocityMultiplier
    {
        get => _velocityMultiplier;
        set => _velocityMultiplier = value;
    }

    private Collider _agentCollider;
    private Flock _flock;
    private float _velocityMultiplier;
    
    void Awake()
    {
        _agentCollider = GetComponent<Collider>();
    }

    public void Initialize(Flock flock)
    {
        _flock = flock;
        _velocityMultiplier = 1f;
    }

    public void Move(Vector2 velocity)
    {
        if(_paused)
            return;
        
        var newPosition = (Vector2)transform.position + velocity * (Time.deltaTime * _velocityMultiplier);

        if ((_oldPosition - newPosition).magnitude < _moveThreshold)
        {
            ChangeAnimationState(AnimationState.Idle);
            return;
        }

        ChangeAnimationState(AnimationState.Running);

        transform.position = new Vector3(newPosition.x, newPosition.y, 2f);
        _oldPosition = newPosition;
        
        transform.up = Vector2.Lerp(transform.up, velocity.normalized, _turnSmoothTime * Time.deltaTime);
    }

    //Todo: Better to generate corpse obj?
    public void Die()
    {
        EventManager.TriggerEvent(Constants.Events.AGENT_DIED, this);
        ChangeAnimationState(AnimationState.Dying);
        Flock.RemoveFromFlock(this);
        GetComponent<Collider>().enabled = false;
        Color c = _spriteRenderer.color;
        c.a = .5f;
        _spriteRenderer.color = c;
    }

    public void ChangeAnimationState(AnimationState newState)
    {
        if (_animator.runtimeAnimatorController == null ||
            _animationState == newState || 
            !_canChangeAnimationState)
            return;
        
        Debug.Log($"Changing animation state from {_animationState} to {newState}");

        _animationState = newState;

        switch (_animationState)
        {
            case AnimationState.Running:
                _animator.SetTrigger(Constants.AnimationTriggers.RUN);
                break;
            case AnimationState.Idle:
                _animator.SetTrigger(Constants.AnimationTriggers.IDLE);
                break;
            case AnimationState.Dying:
                _animator.SetTrigger(Constants.AnimationTriggers.DIE);
                break;
            case AnimationState.Attacking:
                _animator.SetTrigger(Constants.AnimationTriggers.ATTACK);
                break;
        }

        _canChangeAnimationState = false;
        _timePassedSinceAnimationStateChange = 0f;
    }

    private void Update()
    {
        if(!_canChangeAnimationState)
        {
            _timePassedSinceAnimationStateChange += Time.deltaTime;
            if (_timePassedSinceAnimationStateChange >= _animationStateChangeCooldown)
            {
                _canChangeAnimationState = true;
            }
        }
    }
}