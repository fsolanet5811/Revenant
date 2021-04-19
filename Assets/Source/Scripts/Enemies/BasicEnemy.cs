using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicEnemy : AlertableEnemy
{
    protected WaypointMotionProvider _unalertMotionProvider;
    protected AssasinationZone _assasinationZone;

    public Transform[] Waypoints;

    protected override void Awake()
    {
        base.Awake();
        _assasinationZone = GetComponentInChildren<AssasinationZone>();
    }

    protected override void Start()
    {
        // Center the assaination zone around our physical hitbox.
        _assasinationZone.Place(_physicalHitBox.offset);

        base.Start();
    }

    public void Assasinate()
    {
        Damage(CurrentHealth);
    }

    protected override IMotionProvider GetUnalertMotionProvider()
    {
        _unalertMotionProvider = new BounceWaypointMotionProvider(_path, _destinationSetter, transform, Waypoints);
        return _unalertMotionProvider;
    }
}
