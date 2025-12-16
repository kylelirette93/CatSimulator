using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour, IClimbMovement
{
    private IMovementHandler _inputSource;

    public Vector2 CurrentMovementVector => _currentMovementVector;
    private Vector2 _currentMovementVector;

    [Header("Movement Settings")]
    [SerializeField] float movementSpeed = 5f;

    [Header("Climb Settings")]
    private Collider2D _currentSurfaceBounds;
    private Vector3 _climbTargetPosition;

    public bool IsClimbing => _isClimbing;
    public bool _isClimbing = false;
    private bool _isPerformingTween = false;

    private void Awake()
    {
        TryGetComponent<IMovementHandler>(out _inputSource);
        if (_inputSource == null)
        {
            Debug.LogError("PlayerController requires a component that implements IMovementHandler.");
        }
    }

    private void OnEnable()
    {
        if (_inputSource != null)
        {
            _inputSource.MoveInputEvent += HandleMovementInput;
        }
    }

    private void OnDisable()
    {
        if (_inputSource != null)
        {
            _inputSource.MoveInputEvent -= HandleMovementInput;
        }
    }

    private void HandleMovementInput(Vector2 movementVector)
    {
        _currentMovementVector = movementVector;
    }

    private void Update()
    {
        if (_isClimbing)
        {
            HandleClimb(_currentMovementVector);
        }
        else
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if (_currentMovementVector.magnitude > 0.1f)
        {
            Vector3 movement = new Vector3(_currentMovementVector.x, _currentMovementVector.y, 0);
            movement = movement.normalized;
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
        }
    }

    public void ClimbUpSurface(Collider2D surfaceBounds, Vector3 entryPosition)
    {
        transform.DOKill(true);

        _currentSurfaceBounds = surfaceBounds;
        _climbTargetPosition = entryPosition;

        _isClimbing = false;

        Vector3 directionToTarget = (entryPosition - transform.position).normalized;

        Vector3 jumpDestination = transform.position + directionToTarget * 0.5f;

        transform.DOJump(
            jumpDestination,
            jumpPower: 0.2f,
            numJumps: 1,
            duration: 0.3f)
            .SetEase(Ease.OutSine).OnComplete(() =>
            {
                _isClimbing = true;
                transform.DOKill(true);
            });
    }

    public void ClimbDownSurface()
    {
        _currentSurfaceBounds = null;
        _isClimbing = false;
    }

    private void HandleClimb(Vector2 input)
    {
        if (_currentSurfaceBounds == null)
        {
            ClimbDownSurface();
            return;
        }

        //if (input.magnitude < 0.1f) return;

        Vector3 newPosition = transform.position + (Vector3)input.normalized * Time.deltaTime * movementSpeed;

        if (_currentSurfaceBounds.bounds.Contains(newPosition))
        {
            transform.position = newPosition;
        }
        else
        {
            HandleExitJump(input);
        }
    }

    private void HandleExitJump(Vector2 inputDirection)
    {
        if (!_isClimbing) return;

        _isClimbing = false;

        Vector3 destination = transform.position + (Vector3)inputDirection.normalized * 0.5f;

        transform.DOJump(
            destination,
            jumpPower: 0.2f,
            numJumps: 1,
            duration: 0.3f
        ).OnComplete(() =>
        {
            ClimbDownSurface();
        });
    }
}