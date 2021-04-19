using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyId
{
    public Vector3 Id { get; }

    public EnemyId(Vector3 initialEnemyPosition)
    {
        Id = initialEnemyPosition;
    }

    public override bool Equals(object obj)
    {
        return obj is EnemyId eid ? Equals(eid) : base.Equals(obj);
    }

    public bool Equals(EnemyId other)
    {
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(EnemyId a, EnemyId b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(EnemyId a, EnemyId b)
    {
        return !(a == b);
    }
}
