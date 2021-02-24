using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMotionProvider : IMotionProvider
{
    public Vector2 Motion { get; set; }

    public FixedMotionProvider(Vector2 motion)
    {
        Motion = motion;
    }

    public Vector2 GetMotion()
    {
        return Motion;
    }
}
