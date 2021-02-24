using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertableMotionProvider : IMotionProvider
{
    private readonly AlertZone _alertZone;

    public IMotionProvider AlertMotionProvider { get; set; }

    public IMotionProvider UnalertMotionProvider { get; set; }

    public AlertableMotionProvider(AlertZone alertZone, IMotionProvider alertMotionProvider, IMotionProvider unalertMotionProvider)
    {
        _alertZone = alertZone;
        AlertMotionProvider = alertMotionProvider;
        UnalertMotionProvider = unalertMotionProvider;
    }

    public Vector2 GetMotion()
    {
        return _alertZone.IsAlerted ? AlertMotionProvider.GetMotion() : UnalertMotionProvider.GetMotion();
    }
}
