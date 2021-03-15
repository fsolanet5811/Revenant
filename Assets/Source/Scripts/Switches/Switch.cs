using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Switch : MonoBehaviour
{
    /// <summary>
    /// Gets/Sets whether the switch is flipped or not.
    /// </summary>
    public bool Flipped { get; set; }

    /// <summary>
    /// Toggles the switch.
    /// </summary>
    public void Flip()
    {
        Flipped = !Flipped;
    }

}
