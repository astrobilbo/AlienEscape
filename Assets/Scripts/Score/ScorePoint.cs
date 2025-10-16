using UnityEngine;

public class ScorePoint : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private bool _hasScored;
    [SerializeField] private bool logEnabled = false;

    private IScoreSystem _scoreSystem;

    private void Start()
    {
        _scoreSystem = ScoreSystem.Instance;

        if (_scoreSystem != null) return;
        LogError("ScoreSystem not found in GameManager!");
        enabled = false;
    }

    private void OnEnable()
    {
        _hasScored = false;
        Log($"OnEnable - Reset scored flag on {name}");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Log($"Trigger entered by: {other.name}, Tag: {other.tag}, Already scored: {_hasScored}");

        if (_hasScored || _scoreSystem == null) return;
        if (!other.CompareTag(PlayerTag)) return;
        
        _hasScored = true;
        _scoreSystem.AddScore();
        Log($"Score added! New score: {_scoreSystem.GetScore()}");
    }

    private void Log(string message)
    {
        if (!logEnabled) return;

        Debug.Log($"{nameof(ScorePoint)}: " + message);
    }
    
    private static void LogError(string message) => Debug.LogError($"{nameof(ScorePoint)}: " + message);
}