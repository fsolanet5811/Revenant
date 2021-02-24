using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMotionProvider : IMotionProvider
{
    public IMotionProvider CurrentMotionProvider { get; set; }

    public DynamicMotionProvider(IMotionProvider currentMotionProvider)
    {
        CurrentMotionProvider = currentMotionProvider;
    }

    public DynamicMotionProvider() : this(null) { }

    public Vector2 GetMotion()
    {
        return CurrentMotionProvider?.GetMotion() ?? Vector2.zero;
    }
}
