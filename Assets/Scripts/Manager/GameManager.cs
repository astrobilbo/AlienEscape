using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action GameStarted;
    public event Action GameOver;
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject scoreSystem;
    [SerializeField] private bool logEnabled = false;
    public bool IsGameActive { get; private set; }
    
    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (!scoreSystem)
        {
            LogError("ScoreSystem is not assigned!");
            return;
        }

        DontDestroyOnLoad(gameObject);
        Log("GameManager Awake");
    }
    
    private void Start()
    {
        IsGameActive = false;
    }

    private void OnDestroy()
    {
        if (Instance != this) return;
        Instance = null;
        Log("GameManager Destroyed");
    }

    public void StartGame()
    {
        IsGameActive = true;
        GameStarted?.Invoke();
        Log("Game Started");
    }

    public void EndGame()
    {
        IsGameActive = false;
        GameOver?.Invoke();
        Log("Game Over");
    }

    private void Log(string message)
    {
        if (!logEnabled) return;

        Debug.Log($"{nameof(GameManager)}: " + message);
    }
    
    private static void LogError(string message)
    {
        Debug.LogError($"{nameof(GameManager)}: " + message);
    }
}