using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour, IScoreSystem
{
    public static ScoreSystem Instance;

    public event Action ScoreChanged;
    public event Action HighScoreChanged;

    private int _score;
    private int _highScore;
    private const string HighScoreKey = "HighScore";

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
    }

    private void Start()
    {
        GameManager.Instance.GameStarted += ResetScore;
    }
    
    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.GameStarted -= ResetScore;
    }


    private void OnDestroy()
    {
        if (Instance != this) return;
        Instance = null;
    }

    public void AddScore(int amount = 1)
    {
        _score += amount;
        ScoreChanged?.Invoke();

        if (_score > _highScore)
        {
            UpdateHighScore();
        }
    }

    private void UpdateHighScore()
    {
        _highScore = _score;
        HighScoreChanged?.Invoke();
        PlayerPrefs.SetInt(HighScoreKey, _highScore);
        PlayerPrefs.Save();
    }

    public void LoadHighScore()
    {
        _highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        HighScoreChanged?.Invoke();
    }

    public void ResetScore()
    {
        _score = 0;
        ScoreChanged?.Invoke();
    }

    public int GetScore() => _score;
    public int GetHighScore() => _highScore;
}