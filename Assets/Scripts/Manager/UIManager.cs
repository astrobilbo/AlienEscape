using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup) return;
        LogError("CanvasGroup is not assigned!");
    }

    private void Start()
    {
        GameManager.Instance.GameStarted += OnGameStarted;
        GameManager.Instance.GameOver += OnGameOver;
        
        CGActive(!GameManager.Instance.IsGameActive);
    }
    
    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.GameStarted -= OnGameStarted;
        GameManager.Instance.GameOver -= OnGameOver;
    }
    
    private void OnGameStarted()
    {
        CGActive(false);
    }
    
    private void OnGameOver()
    {
        CGActive(true);
    }
    
    private void CGActive(bool isActive)
    {
        if (!canvasGroup) return;
        canvasGroup.alpha = isActive ? 1 : 0;
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }
    
    private static void LogError(string message)
    {
        Debug.LogError($"{nameof(GameManager)}: " + message);
    }
}