using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public int Population => _agents.Count;
    public float SquareLineOfSightRadius => _squareLineOfSightRadius;
    public float SquareAvoidanceRadius => _squareAvoidanceRadius;
    public string FlockName => _flockName;

    public float MaxSpeed => _maxSpeed;

    public List<WeightedBehavior> AllBehaviors => GetAllBehaviors();

    [SerializeField] private string _flockName;
    [SerializeField] FlockAgent _agentPrefab;
    [SerializeField] private List<AbstractCompositeFlockBehavior> _behaviors; 
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
    [SerializeField] protected float _avoidanceRadiusMultiplier = 0.5f;

    [Header("Debug")]
    [SerializeField] private bool _debugShowNeighborRadius = true;
    [SerializeField] private bool _debugShowAvoidanceRadius = true;
    [SerializeField] private bool _debugShowNeighborhoodDensity = true;
    [SerializeField] private bool _debugShowLineOfSightRadius = true;
    
    private float _squareMaxSpeed;
    private float _squareLineOfSightRadius;
    private float _squareNeighborRadius;
    private float _squareAvoidanceRadius;
    private List<FlockAgent> _agents = new List<FlockAgent>();
    
    private const float AGENT_DENSITY = 0.16f;

    private Contexts _contexts;

    public struct Contexts
    {
        public List<Transform> neighborhoodContext;
        public List<Transform> immediateContext;
        public List<Transform> lineOfSightContext;
    }

    public void AddToFlock(FlockAgent agent)
    {
        agent.Flock = this;
        agent.transform.parent = transform;
        agent.Initialize(this);
        _agents.Add(agent);
        
        EventManager.TriggerEvent(Constants.Events.AGENT_ADDED_TO_FLOCK, agent);
    }
    
    public void RemoveFromFlock(FlockAgent agent)
    {
        _agents.Remove(agent);
    }
    
    private List<WeightedBehavior> GetAllBehaviors()
    {
        var allBehaviors = new List<WeightedBehavior>();
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
        UpdateSquareAvoidanceRadius();
    }
    
    protected void UpdateSquareAvoidanceRadius()
    {
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
            
            LoadAgentContexts(agent, ref _contexts);
            
            Vector2 move = Vector2.zero;

            foreach (var behavior in _behaviors)
            {
                move += behavior.CalculateMove(agent, _contexts, this);
            }

            move *= _driveFactor;

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
    
    private void LoadAgentContexts(FlockAgent agent, ref Contexts contexts)
    {
        contexts.neighborhoodContext = new List<Transform>();
        contexts.immediateContext = new List<Transform>();
        contexts.lineOfSightContext = new List<Transform>();
        var contextColliders = Physics.OverlapSphere(agent.transform.position, _lineOfSightRadius);
        foreach (var collider in contextColliders)
        {
            if(collider != agent.AgentCollider)
            {
                var sqMag = Vector2.SqrMagnitude(collider.ClosestPoint(agent.transform.position) - agent.transform.position);
                if(sqMag > _squareLineOfSightRadius)
                    continue;
                contexts.lineOfSightContext.Add(collider.transform);
                if (sqMag <= _squareNeighborRadius)
                {
                    contexts.neighborhoodContext.Add(collider.transform);
                }
                else continue;
                if (sqMag <= _squareAvoidanceRadius)
                {
                    contexts.immediateContext.Add(collider.transform);
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        if (_debugShowAvoidanceRadius || _debugShowNeighborRadius || _debugShowLineOfSightRadius)
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
                if (_debugShowLineOfSightRadius)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireSphere(agent.transform.position, _lineOfSightRadius);
                }
            }
        }
    }
    
}
