using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    public Pose Pose { get; set; }

    public float CurrentSpeed { get; set; }

    public Direction CurrentDirection { get; set; }

    public float CurrentHealth { get; set; }

    public bool WasDespawned { get; }

    private EnemyState(bool wasDespawned)
    {
        WasDespawned = wasDespawned;
    }

    public EnemyState() : this(false) { }

    public static EnemyState Despawned()
    {
        return new EnemyState(true);
    }
}
