using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;

public class BounceWaypointMotionProvider : WaypointMotionProvider
{
    private static readonly int FORWARD_DIRECTION = 1;
    private static readonly int BACKWARD_DIRECTION = -1;

    private int _direction;
    private bool _isPausedAtEnd;

    /// <summary>
    /// Gets/Sets the amount of time that the enemies wait when they reach the end of the waypoints.
    /// </summary>
    public float EndPauseSeconds { get; set; }

    public BounceWaypointMotionProvider(AIPath path, AIDestinationSetter destinationSetter, Transform self, IEnumerable<Transform> waypoints)
        : base(path, destinationSetter, self, waypoints)
    {
        _direction = FORWARD_DIRECTION;
        EndPauseSeconds = 1;
    }

    protected override int GetNextWaypoint(int currentWaypoint)
    {
        // When we hit the ends, we gotta wait a bit.
        if (_isPausedAtEnd)
            return currentWaypoint;

        if(IsEnd(currentWaypoint))
        {
            // Change direction so we bounce back through the waypoints.
            _direction *= -1;

            Task.Run(() =>
            {
                _isPausedAtEnd = true;
                Thread.Sleep((int)(EndPauseSeconds * 1000));
                _isPausedAtEnd = false;
            });

            // Telling the base class to go to the same waypoint it is at will stop it from moving.
            return currentWaypoint;
        }

        return currentWaypoint + _direction;
    }

    private bool IsEnd(int waypoint)
    {
        return (_direction == FORWARD_DIRECTION && waypoint == _waypoints.Length - 1) || (_direction == BACKWARD_DIRECTION && waypoint == 0);
    }
}
