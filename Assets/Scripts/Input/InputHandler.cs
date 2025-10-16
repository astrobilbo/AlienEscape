using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, IInputHandler
{
    public event Action Jump;
    private InputSystem_Actions _inputActions;

    private void Awake() => _inputActions = new InputSystem_Actions();

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Jump.canceled += OnJumpPerformed;
    }

    private void OnDisable()
    {
        _inputActions.Player.Jump.canceled -= OnJumpPerformed;
        _inputActions.Player.Disable();
    }

    private void OnDestroy() => _inputActions?.Dispose();

    private void OnJumpPerformed(InputAction.CallbackContext obj) => Jump?.Invoke();
}