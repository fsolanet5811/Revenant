using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectProjectile : Projectile
{
    private readonly FixedMotionProvider _motionProvider;

    public Vector2 Direction
    {
        get
        {
            return _motionProvider.Motion;
        }
        set
        {
            _motionProvider.Motion = value;
        }
    }

    public DirectProjectile()
    {
        _motionProvider = new FixedMotionProvider(Vector2.zero);
    }

    protected override void Start()
    {
        base.Start();
        _motionProvider.Motion = transform.right;
    }

    protected override IMotionProvider GetMotionProvider()
    {
        return _motionProvider;
    }
}
