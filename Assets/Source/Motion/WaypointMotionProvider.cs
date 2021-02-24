using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WaypointMotionProvider : AStarMotionProvider
{
    protected readonly Transform[] _waypoints;
    private int _nextWaypoint;
    
    public float Threshold { get; set; }

    protected WaypointMotionProvider(AIPath path, AIDestinationSetter destinationSetter, Transform self, IEnumerable<Transform> waypoints)
        : base(path, destinationSetter, self)
    {
        Threshold = 0.1f;
        _waypoints = waypoints?.ToArray();
        Reset();
    }

    public void Reset()
    {
        // Instruct the provider to go to the first waypoint.
        _nextWaypoint = 0;
        Target = HasWaypoints() ? _waypoints[_nextWaypoint] : null;
    }

    public override Vector2 GetMotion()
    {
        // No waypoints means don't move.
        if (!HasWaypoints())
            return Vector2.zero;

        // Ensure we have the right target before getting the motion.
        // This can be modified by another object that has a reference to the same AI destination setter.
        Target = _waypoints[_nextWaypoint];

        // Go straight to the next waypoint.
        Vector2 toWaypoint = _waypoints[_nextWaypoint].position - _self.position;

        // Depending on the threshold, we either go to this point or move on to the next one.
        if(toWaypoint.magnitude < Threshold)
        {
            // Grab the next waypoint to go to.
            int next = GetNextWaypoint(_nextWaypoint);

            // To prevent an infinte loop, we will not move if they tell us to go to the same waypoint.
            if (next == _nextWaypoint)
                return Vector2.zero;

            // Setting our next waypoint and calling this again should get us the motion.
            _nextWaypoint = next;
            Target = _waypoints[_nextWaypoint];
            return GetMotion();
        }

        // Just move here.
        // Calling the base version will use the a star.
        return base.GetMotion();
    }

    protected abstract int GetNextWaypoint(int currentWaypoint);

    protected bool HasWaypoints()
    {
        return _waypoints?.Any() ?? false;
    }
}
