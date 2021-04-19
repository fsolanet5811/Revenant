using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Switch : MonoBehaviour
{
    /// <summary>
    /// Gets/Sets whether the switch is flipped or not.
    /// </summary>
    public bool Flipped { get; set; }
    private bool flipLock = true;

    /// <summary>
    /// Toggles the switch.
    /// </summary>
    public void Flip()
    {
        Flipped = !Flipped;
        flipLock = !flipLock;
    }
    //please push
    [SerializeField] AnimateSwitch sw;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerController>() != null && flipLock)
        {
            Flip();
            SwitchController.flippedCount += 1;
            sw.flipSwitchOn();
            if (SwitchController.flippedCount > 7)
               SceneManager.LoadScene("Win");
         }
    }
}
