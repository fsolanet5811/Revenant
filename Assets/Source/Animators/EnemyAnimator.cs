using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator
{
    private readonly Animator _animator;

    private bool IsMoving
    {
        get
        {
            return _animator.GetBool(nameof(IsMoving));
        }
        set
        {
            _animator.SetBool(nameof(IsMoving), value);
        }
    }

    private Direction Direction
    {
        get
        {
            return (Direction)_animator.GetFloat(nameof(Direction));
        }
        set
        {
            _animator.SetFloat(nameof(Direction), (int)value);
        }
    }

    private bool IsAttacking
    {
        get
        {
            return _animator.GetBool(nameof(IsAttacking));
        }
        set
        {
            _animator.SetBool(nameof(IsAttacking), value);
        }
    }

    public EnemyAnimator(Animator enemyAnimator)
    {
        _animator = enemyAnimator;
    }

    public void AnimateMove(Vector2 direction)
    {
        IsMoving = true;
        AnimateDirection(direction);
    }

    public void AnimateIdle(Vector2 direction)
    {
        AnimateIdle();
        AnimateDirection(direction);
    }

    public void AnimateDirection(Vector2 direction)
    {
        Direction = DirectionFromVector(direction);
    }

    public void AnimateAttack()
    {
        IsAttacking = true;
    }

    public void AnimateIdle()
    {
        IsMoving = false;
    }

    private static Direction DirectionFromVector(Vector2 v)
    {
        // Grab the stronger of the two axes. 
        float absX = Mathf.Abs(v.x);
        float absY = Mathf.Abs(v.y);

        if (absX > absY)
            return v.x < 0 ? Direction.Left : Direction.Right;

        return v.y < 0 ? Direction.Down : Direction.Up;
    }
}
