using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WaypointMotionProvider : AStarMotionProvider
{
    protected readonly Transform[] _waypoints;
    private int? _nextWaypoint;

    public float Threshold { get; set; }

    protected WaypointMotionProvider(AIPath path, AIDestinationSetter destinationSetter, Transform self, IEnumerable<Transform> waypoints)
        : base(path, destinationSetter, self)
    {
        Threshold = 0.1f;
        _waypoints = waypoints?.ToArray();
    }

    public void Reset()
    {
        // Instruct the provider to go to the first waypoint.
        _nextWaypoint = GetInitialWaypoint();
        Target = HasWaypoints() ? _waypoints[_nextWaypoint.Value] : null;
    }

    public override Vector2 GetMotion()
    {
        // We have to wait till now to call the reset so that sub class coinstructors can add logic first.
        if (!_nextWaypoint.HasValue)
            Reset();

        // No waypoints means don't move.
        if (!HasWaypoints())
            return Vector2.zero;

        // Ensure we have the right target before getting the motion.
        // This can be modified by another object that has a reference to the same AI destination setter.
        Target = _waypoints[_nextWaypoint.Value];

        // Go straight to the next waypoint.
        Vector2 toWaypoint = _waypoints[_nextWaypoint.Value].position - _self.position;

        // Depending on the threshold, we either go to this point or move on to the next one.
        // There are times (I think when we are moving fast) that the astar might think we are closer than we actually are, and so it does not want to move.
        // Let's double check that.
        // It sould return 0 motion when that is the case.
        Vector2 astarMotion;
        if (toWaypoint.magnitude < Threshold || (astarMotion = base.GetMotion()) == Vector2.zero)
        {
            // Grab the next waypoint to go to.
            int next = GetNextWaypoint(_nextWaypoint.Value);

            // To prevent an infinte loop, we will not move if they tell us to go to the same waypoint.
            if (next == _nextWaypoint)
                return Vector2.zero;

            // Setting our next waypoint and calling this again should get us the motion.
            _nextWaypoint = next;
            Target = _waypoints[_nextWaypoint.Value];

            // It takes astar a frame to update, so return no motion for now.
            return Vector2.zero;
        }

        // Just move here.
        return astarMotion;
    }

    protected abstract int GetNextWaypoint(int currentWaypoint);

    protected virtual int GetInitialWaypoint()
    {
        return 0;
    }

    protected bool HasWaypoints()
    {
        return _waypoints?.Any() ?? false;
    }
}
