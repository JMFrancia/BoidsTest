using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    public Collider AgentCollider => _agentCollider;

    public bool Paused
    {
        get => _paused;
        set => _paused = value;
    }

    [SerializeField] private float _turnSmoothTime = 10f;
    [SerializeField] private float _moveThreshold = 0.5f;
    [SerializeField] private float _turnThreshold = .05f;

    private Vector2 _oldPosition;
    private Vector2 _oldDirection;
    private bool _paused;
    
    public Flock Flock
    {
        get => _flock;
        set => _flock = value;
    }
    
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
        
        if (newPosition == _oldPosition || (_oldPosition - newPosition).magnitude < _moveThreshold)
            return;
        
        transform.position = newPosition;
        _oldPosition = newPosition;
        
        var newDirection = Vector2.Lerp(transform.up, velocity.normalized, _turnSmoothTime * Time.deltaTime);
        // if (Vector2.Angle(newDirection, _oldDirection) < _turnThreshold)
        //     return;

        transform.up = newDirection;
        _oldDirection = newDirection;
    }
}