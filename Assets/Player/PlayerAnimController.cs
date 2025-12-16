using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAnimController : MonoBehaviour
{
    private IMovementHandler _inputSource;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        TryGetComponent<IMovementHandler>(out _inputSource);
        TryGetComponent<Animator>(out _animator);
        TryGetComponent<SpriteRenderer>(out _spriteRenderer);
        if (_inputSource == null)
        {
            Debug.LogError("PlayerController requires a component that implements IMovementHandler.");
        }
    }

    private void OnEnable()
    {
        if (_inputSource != null)
        {
            _inputSource.MoveInputEvent += UpdateAnimation;
        }
    }

    private void OnDisable()
    {
        if (_inputSource != null)
        {
            _inputSource.MoveInputEvent -= UpdateAnimation;
        }
    }

    private void UpdateAnimation(Vector2 input)
    {
        _animator.SetBool("IsMoving", input.magnitude > 0.1f);
        Flip(input);
    }

    private void Flip(Vector2 input)
    {
        if (input.magnitude > 0.1f)
        {
            if (input.x > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (input.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }
    }
}
