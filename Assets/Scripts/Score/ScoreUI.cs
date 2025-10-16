using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private IScoreSystem _scoreSystem;

    private void Start()
    {
        if (!scoreText)
        {
            LogError($"requires a reference to a {nameof(TextMeshProUGUI)} for score display.");
            enabled = false;
            return;
        }

        if (!highScoreText)
        {
            LogError($"requires a reference to a {nameof(TextMeshProUGUI)} for high score display.");
            enabled = false;
            return;
        }
        
        _scoreSystem = ScoreSystem.Instance;
        if (_scoreSystem == null)
        {
            LogError("ScoreSystem not found in GameManager!");
            enabled = false;
            return;
        }

        UpdateScoreText();
        UpdateHighScoreText();

        _scoreSystem.ScoreChanged += UpdateScoreText;
        _scoreSystem.HighScoreChanged += UpdateHighScoreText;
    }

    private void OnDestroy()
    {
        if (_scoreSystem == null) return;
        _scoreSystem.ScoreChanged -= UpdateScoreText;
        _scoreSystem.HighScoreChanged -= UpdateHighScoreText;
    }

    private void UpdateScoreText() => scoreText.text = $"Score: {_scoreSystem.GetScore()}";
    private void UpdateHighScoreText() => highScoreText.text = $"High Score: {_scoreSystem.GetHighScore()}";

    private static void LogError(string message) => Debug.LogError($"{nameof(ScoreUI)}: " + message);
}