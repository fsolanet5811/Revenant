using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AStarMotionProvider : IMotionProvider
{
    private readonly AIPath _path;
    private readonly AIDestinationSetter _destinationSetter;
    protected readonly Transform _self;

    protected Transform Target
    {
        get
        {
            return _destinationSetter.target;
        }
        set
        {
            _destinationSetter.target = value;
        }
    }

    protected AStarMotionProvider(AIPath path, AIDestinationSetter destinationSetter, Transform self)
    {
        _path = path;
        _path.canMove = false;
        _path.enableRotation = false;
        _destinationSetter = destinationSetter;
        _self = self;
    }

    public virtual Vector2 GetMotion()
    {
        _path.MovementUpdate(Time.deltaTime, out Vector3 nextPosition, out _);
        return nextPosition - _self.position;
    }
}
