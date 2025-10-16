using UnityEngine;

public class ObstacleDestroyer : MonoBehaviour
{
    private const string PlayerTag = "Player";
    [SerializeField] private bool logEnabled = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Log($"Trigger entered by: {other.name}, Tag: {other.tag}");

        if (!other.CompareTag(PlayerTag)) return;
        Log("Player collision detected!");

        if (GameManager.Instance == null)
        {
            LogError("GameManager.Instance is null!");
            return;
        }

        GameManager.Instance.EndGame();
    }
    
    private void Log(string message)
    {
        if (!logEnabled) return;

        Debug.Log($"{nameof(ObstacleDestroyer)}: " + message);
    }
    
    private static void LogError(string message)
    {
        Debug.LogError($"{nameof(ObstacleDestroyer)}: " + message);
    }
}