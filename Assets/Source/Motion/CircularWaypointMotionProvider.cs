using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularWaypointMotionProvider : WaypointMotionProvider
{
    public CircularWaypointMotionProvider(AIPath path, AIDestinationSetter destinationSetter, Transform self, IEnumerable<Transform> waypoints)
        : base(path, destinationSetter, self, waypoints) { }

    protected override int GetNextWaypoint(int currentWaypoint)
    {
        // Basically we just give them the next waypoint and it loops around to the beginning.
        return ++currentWaypoint >= _waypoints.Length ? 0 : currentWaypoint;
    }
}
