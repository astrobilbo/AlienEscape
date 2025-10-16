using UnityEngine;
using UnityEngine.Pool;

public class ObstacleFactory : MonoBehaviour
{
    [SerializeField] private ObstacleHandler obstaclePrefab;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private float minSpawnHeight = -2f;
    [SerializeField] private float maxSpawnHeight = 2f;
    [SerializeField] private bool logEnabled = false;
    
    private ObjectPool<ObstacleHandler> _obstaclePool;
    
    private void Awake()
    {
        Log("Awake called");
        Log($"Prefab assigned: {obstaclePrefab != null}");
        Log($"Spawn position assigned: {spawnPosition != null}");
        
        _obstaclePool = new ObjectPool<ObstacleHandler>(
            CreatePooledItem, 
            OnTakeFromPool, 
            OnReturnedToPool, 
            OnDestroyPoolObject, 
            collectionCheck: true, 
            defaultCapacity: 10, 
            maxSize: 20
        );
        
        Log("Pool created successfully");
    }

    public ObstacleHandler SpawnObstacle(bool enableLogs)
    {
        Log("SpawnObstacle called");
        
        if (_obstaclePool == null)
        {
            LogError("Pool is null!");
            return null;
        }
        
        var obstacle = _obstaclePool.Get();
        Log($"Got obstacle from pool: {obstacle}");
        
        obstacle.SetLogging(enableLogs);
        
        if (!spawnPosition)
        {
            LogError("Spawn position is null!");
            return obstacle;
        }
        
        var spawnPos = spawnPosition.position;
        float randomY = Random.Range(minSpawnHeight, maxSpawnHeight);
        spawnPos.y = randomY;
        obstacle.transform.position = spawnPos;
        Log($"Obstacle spawned at position: {spawnPos} (random Y: {randomY})");
        
        return obstacle;
    }

    public void ReturnObstacle(ObstacleHandler obstacle)
    {
        if (!obstacle || _obstaclePool == null) return;
        Log($"Returning obstacle to pool: {obstacle}");
        _obstaclePool.Release(obstacle);
    }

    private ObstacleHandler CreatePooledItem()
    {
        Log("Creating new pooled item");
        
        if (!obstaclePrefab)
        {
            LogError("Cannot create obstacle - prefab is null!");
            return null;
        }
        
        var obstacle = Instantiate(obstaclePrefab);
        Log($"Instantiated obstacle: {obstacle}, Name: {obstacle.name}");
        
        obstacle!.Initialize(this);
        return obstacle;
    }

    private void OnTakeFromPool(ObstacleHandler obj)
    {
        if (!obj) return;
        Log($"Taking from pool: {(obj.name)}");
        obj.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(ObstacleHandler obj)
    {
        if (!obj) return;
        Log($"Returned to pool: {(obj.name)}");
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(ObstacleHandler obj)
    {
        if (!obj) return;
        Log($"Destroying pool object: {(obj.name)}");
        Destroy(obj.gameObject);
    }
    
    private void Log(string message)
    {
        if (!logEnabled) return;

        Debug.Log($"{nameof(ObstacleFactory)}: " + message);
    }
    
    private static void LogError(string message)
    {
        Debug.LogError($"{nameof(ObstacleFactory)}: " + message);
    }
}