using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPath), typeof(AIDestinationSetter))]
public abstract class BasicEnemy : Enemy
{
    protected FollowerMotionProvider _alertMotionProvider;
    protected WaypointMotionProvider _unalertMotionProvider;
    protected AlertZone _alertZone;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Transform[] _waypoints;

    public bool IsAlerted
    {
        get
        {
            return _alertZone.IsAlerted;
        }
    }

    protected override void Start()
    {
        _alertZone = GetComponentInChildren<AlertZone>();
        _alertZone.AlertTarget = _target;
        base.Start();
    }

    protected override IMotionProvider GetMotionProvider()
    {
        AIPath path = GetComponent<AIPath>();
        AIDestinationSetter destinationSetter = GetComponent<AIDestinationSetter>();
        _alertMotionProvider = new FollowerMotionProvider(path, destinationSetter, transform, _target);
        _unalertMotionProvider = new CircularWaypointMotionProvider(path, destinationSetter, transform, _waypoints);
        return new AlertableMotionProvider(_alertZone, _alertMotionProvider, _unalertMotionProvider);
    }
}
