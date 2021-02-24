using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FollowerMotionProvider : AStarMotionProvider
{
    private Transform _target;

    public new Transform Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
            base.Target = value;
        }
    }

    public FollowerMotionProvider(AIPath path, AIDestinationSetter destinationSetter, Transform self, Transform target) : base(path, destinationSetter, self)
    {
        Target = target;
    }

    public FollowerMotionProvider(AIPath path, AIDestinationSetter destinationSetter, Transform self) : this(path, destinationSetter, self, null) { }

    public override Vector2 GetMotion()
    {
        // Ensure we have the right target before getting the motion.
        // This can be modified by another object that has a reference to the same AI destination setter.
        Target = _target;
        return base.GetMotion();
    }
}

