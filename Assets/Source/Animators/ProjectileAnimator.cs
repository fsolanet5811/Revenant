using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAnimator
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

    public ProjectileAnimator(Animator projectileAnimator)
    {
        _animator = projectileAnimator;
    }

    public void AnimateMove()
    {
        IsMoving = true;
    }

    public void AnimateExplosion()
    {
        IsMoving = false;
    }
}
