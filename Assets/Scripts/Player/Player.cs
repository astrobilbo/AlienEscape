using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;

    private Camera _camera;
    private IInputHandler _inputHandler;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _initialPosition;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inputHandler = GetComponent<IInputHandler>();
        _camera = Camera.main;

        if (!_rigidbody2D)
        {
            LogError($"{nameof(Player)} requires a {nameof(Rigidbody2D)} component");
            enabled = false;
            return;
        }

        if (_inputHandler == null)
        {
            LogError($"{nameof(Player)} requires a component that implements {nameof(IInputHandler)}");
            enabled = false;
            return;
        }

        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        _initialPosition = transform.position;
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        if (_inputHandler != null)
        {
            _inputHandler.Jump += OnJump;
        }

        if (GameManager.Instance == null) return;

        GameManager.Instance.GameStarted += OnStartGame;
        GameManager.Instance.GameOver += OnGameOver;

        if (!GameManager.Instance.IsGameActive) return;
        OnStartGame();
    }

    private void Update()
    {
        if (_rigidbody2D.bodyType != RigidbodyType2D.Dynamic) return;

        CheckCameraBounds();
    }

    private void CheckCameraBounds()
    {
        if (_camera == null) return;

        var playerPosition = transform.position;
        float cameraHeight = _camera.orthographicSize;
        float cameraWidth = cameraHeight * _camera.aspect;
        var cameraPosition = _camera.transform.position;

        bool isOutOfBounds = playerPosition.y > cameraPosition.y + cameraHeight ||
                             playerPosition.y < cameraPosition.y - cameraHeight ||
                             playerPosition.x > cameraPosition.x + cameraWidth || 
                             playerPosition.x < cameraPosition.x - cameraWidth;

        if (isOutOfBounds && GameManager.Instance)
        {
            GameManager.Instance.EndGame();
        }
    }

    private void OnDestroy()
    {
        if (_inputHandler != null)
        {
            _inputHandler.Jump -= OnJump;
        }

        if (GameManager.Instance == null) return;
        GameManager.Instance.GameStarted -= OnStartGame;
        GameManager.Instance.GameOver -= OnGameOver;
    }

    private void OnStartGame()
    {
        transform.position = _initialPosition;
        _rigidbody2D.linearVelocity = Vector2.zero;
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        OnJump();
    }

    private void OnGameOver()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        _rigidbody2D.linearVelocity = Vector2.zero;
    }

    private void OnJump()
    {
        if (_rigidbody2D.bodyType != RigidbodyType2D.Dynamic) return;

        _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, jumpForce);

        if (_camera != null && transform.position.y > _camera.orthographicSize)
        {
            GameManager.Instance.EndGame();
        }
    }

    private static void LogError(string message) => Debug.LogError($"{nameof(Player)}: " + message);
}