using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum Direction
{
    Right = 0,
    Up = 1,
    Left = 2,
    Down = 3
}

public static class DirectionUtilities
{
    public static Direction DirectionFromVector(Vector2 v)
    {
        // Grab the stronger of the two axes. 
        float absX = Mathf.Abs(v.x);
        float absY = Mathf.Abs(v.y);

        if (absX > absY)
            return v.x < 0 ? Direction.Left : Direction.Right;

        return v.y < 0 ? Direction.Down : Direction.Up;
    }

    public static Vector2 VectorFromDirection(Direction d)
    {
        return d switch
        {
            Direction.Down => new Vector2(0, -1),
            Direction.Up => new Vector2(0, 1),
            Direction.Right => new Vector2(1, 0),
            Direction.Left => new Vector2(-1, 0),
            _ => throw new NotImplementedException($"Unimplemented direction {d}."),
        };
    }

    public static float DegreesFromDirection(Direction d)
    {
        return Vector2.SignedAngle(Vector2.right, VectorFromDirection(d));
    }

    public static void RotateTransformTowardsDirection(Transform transform, Direction direction)
    {
        transform.Rotate(new Vector3(0, 0, DegreesFromDirection(direction) - transform.rotation.eulerAngles.z));
    }
}
