using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private ObstacleFactory obstacleFactory;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float initialDelay = 1f;
    [SerializeField] private bool logEnabled = false;
    [SerializeField] private bool debugSpawnedObstacles = false;

    private float _nextSpawnTime;
    private bool _isSpawning;

    private void Awake()
    {
        Log("Awake called");
        Log($"Factory assigned: {obstacleFactory != null}");
    }

    private void Start()
    {
        Log("Start called");
        
        if (!obstacleFactory)
        {
            LogError($"{nameof(ObstacleFactory)} is not assigned!");
            enabled = false;
            return;
        }
        
        if (!GameManager.Instance)
        {
            LogError("GameManager.Instance is null in Start!");
            enabled = false;
            return;
        }
        
        GameManager.Instance.GameStarted += StartSpawning;
        GameManager.Instance.GameOver += StopSpawning;
        
        Log($"Configuration - Interval: {spawnInterval}s, Initial Delay: {initialDelay}s");
        Log("Subscribed to GameManager events");
    }

    private void OnDestroy()
    {
        Log("OnDestroy called");

        if (GameManager.Instance == null) return;
        GameManager.Instance.GameStarted -= StartSpawning;
        GameManager.Instance.GameOver -= StopSpawning;
    }

    private void Update()
    {
        if (!_isSpawning) return;

        if (!(Time.time >= _nextSpawnTime)) return;
        Log($"Spawning obstacle at time: {Time.time}");
        obstacleFactory.SpawnObstacle(debugSpawnedObstacles);
        _nextSpawnTime = Time.time + spawnInterval;
        Log($"Next spawn scheduled at: {_nextSpawnTime}");
    }

    private void StartSpawning()
    {
        _isSpawning = true;
        _nextSpawnTime = Time.time + initialDelay;
        Log($"Started spawning. First spawn at: {_nextSpawnTime}");
    }

    private void StopSpawning()
    {
        _isSpawning = false;
        Log("Stopped spawning");
    }
    
    private void Log(string message)
    {
        if (!logEnabled) return;

        Debug.Log($"{nameof(ObstacleSpawner)}: " + message);
    }
    
    private static void LogError(string message)
    {
        Debug.LogError($"{nameof(ObstacleSpawner)}: " + message);
    }
}