using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Input.IPlayerActions, IMovementHandler
{
    private Input playerInput;

    void Awake()
    {
        InitializeInput();
    }

    void InitializeInput()
    {
        #region Initialize Player Input
        try
        {
            playerInput = new Input();
            playerInput.Player.SetCallbacks(this);
            playerInput.Player.Enable();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error initializing input manager: {e.Message}");
        }
        #endregion
    }

    #region Input Events
    public event Action<Vector2> MoveInputEvent;
    #endregion

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInputEvent?.Invoke(context.ReadValue<Vector2>());
    }

    void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.Player.Enable();
        }
    }

    void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.Player.Disable();
        }
    }
}
public interface IMovementHandler
{
    event Action<Vector2> MoveInputEvent;
}
