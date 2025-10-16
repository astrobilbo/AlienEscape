using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float distanceToRemove = 15f;
    private bool logEnabled = false;

    private ObstacleFactory _obstacleFactory;
    private bool _isMoving;
    private bool _isInPool;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(ObstacleFactory factory)
    {
        _obstacleFactory = factory;
        Log($"Initialized with factory: {factory != null}");
    }
    
    public void SetLogging(bool shouldLog)
    {
        logEnabled = shouldLog;
    }

    private void Start()
    {
        Log($"Start called. GameManager exists: {GameManager.Instance != null}");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameStarted += OnGameStarted;
            GameManager.Instance.GameOver += OnGameOver;
            
            if (GameManager.Instance.IsGameActive)
            {
                StartMoving();
            }
        }
        else
        {
            LogWarning("GameManager.Instance is null in Start");
        }
    }

    private void OnEnable()
    {
        _isInPool = false;

        if (GameManager.Instance == null || !GameManager.Instance.IsGameActive) return;
        StartMoving();
        Log("OnEnable: Game is active, started moving");
    }

    private void OnDestroy()
    {
        Log("OnDestroy called");

        if (GameManager.Instance == null) return;
        GameManager.Instance.GameStarted -= OnGameStarted;
        GameManager.Instance.GameOver -= OnGameOver;
    }

    private void FixedUpdate()
    {
        if (!_isMoving) return;

        if (_rb)
        {
            _rb.linearVelocity = new Vector2(speed, _rb.linearVelocity.y);
        }
        else
        {
            transform.Translate(Vector3.right * (speed * Time.fixedDeltaTime));
        }

        if (!(transform.position.x > distanceToRemove)) return;
        Log($"Reached removal distance at x={transform.position.x}");
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (_isInPool)
        {
            Log("Already in pool, skipping return");
            return;
        }
        
        Log($"Returning to pool. Factory exists: {_obstacleFactory}");
        _isMoving = false;
        _isInPool = true;
        
        if (_obstacleFactory)
        {
            _obstacleFactory.ReturnObstacle(this);
        }
    }

    private void OnGameStarted() => StartMoving();

    private void OnGameOver()
    {
        StopMoving();
        ReturnToPool();
    }

    private void StartMoving()
    {
        _isMoving = true;
        Log("Started moving");
    }

    public void StopMoving()
    {
        _isMoving = false;
        
        if (_rb)
        {
            _rb.linearVelocity = Vector2.zero;
        }
        
        Log("Stopped moving");
    }
    
    private void Log(string message)
    {
        if (!logEnabled) return;

        Debug.Log($"{nameof(ObstacleHandler)}: " + message);
    }
    
    private static void LogWarning(string message)
    {
        Debug.LogWarning($"{nameof(ObstacleHandler)}: " + message);
    }
}