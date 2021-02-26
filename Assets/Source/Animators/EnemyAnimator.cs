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

    public void AnimateMove(Direction direction)
    {
        IsMoving = true;
        AnimateDirection(direction);
    }

    public void AnimateIdle(Direction direction)
    {
        AnimateIdle();
        AnimateDirection(direction);
    }

    public void AnimateDirection(Direction direction)
    {
        Direction = direction;
    }

    public void AnimateAttack()
    {
        IsAttacking = true;
    }

    public void AnimateIdle()
    {
        IsMoving = false;
    }
}
