using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [SerializeField]
    private Switch[] _switches;

    public bool AreAllSwitchesFlipped()
    {
        return _switches?.FirstOrDefault(s => s.Flipped) != null;
    }
}
