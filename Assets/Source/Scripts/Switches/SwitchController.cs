using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public static SwitchController switchController;

    [SerializeField]
    public static int flippedCount = 0;

    [SerializeField]
    private static Switch[] _switches;

    public static bool AreAllSwitchesFlipped()
    {
        return _switches?.FirstOrDefault(s => s.Flipped) != null;
    }
}
