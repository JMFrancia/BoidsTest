using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public float SquareLineOfSightRadius => _squareLineOfSightRadius;
    public float SquareAvoidanceRadius => _squareAvoidanceRadius;
    public string FlockName => _flockName;

    public float MaxSpeed => _maxSpeed;

    public List<CompositeBehavior.WeightedBehavior> AllBehaviors => GetAllBehaviors();

    [SerializeField] private string _flockName;
    [SerializeField] FlockAgent _agentPrefab;
    [SerializeField] private List<CompositeBehavior> _behaviors; 
    [Range(1, 500)]
    [SerializeField] private int _startingCount = 250;
    [Range(1f, 100f)]
    [SerializeField] private float _driveFactor = 10f;
    [Range(1f, 100f)]
    [SerializeField] private float _maxSpeed = 5f;
    [Range(5f, 50f)]
    [SerializeField] private float _lineOfSightRadius = 10f;
    [Range(1f, 10f)]
    [SerializeField] private float _neighborRadius = 1.5f;
    [Range(0f, 1f)]
    [SerializeField] private float _avoidanceRadiusMultiplier = 0.5f;

    [Header("Debug")]
    [SerializeField] private bool _debugShowNeighborRadius = true;
    [SerializeField] private bool _debugShowAvoidanceRadius = true;
    [SerializeField] private bool _debugShowNeighborhoodDensity = true;
    
    private float _squareMaxSpeed;
    private float _squareLineOfSightRadius;
    private float _squareNeighborRadius;
    private float _squareAvoidanceRadius;
    private List<FlockAgent> _agents = new List<FlockAgent>();
    
    private const float AGENT_DENSITY = 0.16f;

    public void AddToFlock(FlockAgent agent)
    {
       // agent.Flock.RemoveFromFlock(agent);
        agent.Flock = this;
        agent.transform.parent = transform;
        agent.Initialize(this);
        _agents.Add(agent);
        EventManager.TriggerEvent(Constants.Events.FLOCK_CHANGED, agent.gameObject);
    }
    
    public void RemoveFromFlock(FlockAgent agent)
    {
        _agents.Remove(agent);
    }
    
    private List<CompositeBehavior.WeightedBehavior> GetAllBehaviors()
    {
        var allBehaviors = new List<CompositeBehavior.WeightedBehavior>();
        foreach (var behavior in _behaviors)
        {
            allBehaviors.AddRange(behavior.Behaviors);
        }
        return allBehaviors;
    }

    private void Awake()
    {
        _squareMaxSpeed = _maxSpeed * _maxSpeed;
        _squareLineOfSightRadius = _lineOfSightRadius * _lineOfSightRadius;
        _squareNeighborRadius = _neighborRadius * _neighborRadius;
        _squareAvoidanceRadius = _squareNeighborRadius * _avoidanceRadiusMultiplier * _avoidanceRadiusMultiplier;
    }

    void Start()
    {
        GenerateFlock();
    }

    private void GenerateFlock()
    {
        for (int i = 0; i < _startingCount; i++)
        {
            var newAgent = Instantiate(_agentPrefab, 
                transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * _startingCount * AGENT_DENSITY, 
                Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0f, 360f)), 
                transform);
            newAgent.Initialize(this);
            newAgent.name = $"{_flockName} agent {i}";
            _agents.Add(newAgent);
        }
    }
    
    private void Update()
    {
        foreach (var agent in _agents)
        {
            if(agent.Paused)
                continue;

            var context = GetNearbyObjects(agent);
            Vector2 move = Vector2.zero;

            foreach (var behavior in _behaviors)
            {
                move += behavior.CalculateMove(agent, context, this);
            }

            move *= _driveFactor;
            
            if (_debugShowNeighborhoodDensity)
            {
                // FOR DEMO ONLY
                agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);
            }
            
            // Limit speed
            if (move.sqrMagnitude > _squareMaxSpeed)
            {
                move = move.normalized * _maxSpeed;
            }
            
            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        var context = new List<Transform>();
        var contextColliders = Physics.OverlapSphere(agent.transform.position, _neighborRadius);
        foreach (var collider in contextColliders)
        {
            if(collider != agent.AgentCollider)
            {
                context.Add(collider.transform);
            }
        }
        return context;
    }
    
    private void OnDrawGizmos()
    {
        if (_debugShowAvoidanceRadius || _debugShowNeighborRadius)
        {
            foreach (var agent in _agents)
            {
                if (_debugShowNeighborRadius)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(agent.transform.position, _neighborRadius);
                }
                if (_debugShowAvoidanceRadius)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(agent.transform.position, _neighborRadius * _avoidanceRadiusMultiplier);
                }
            }
        }
    }
    
}
