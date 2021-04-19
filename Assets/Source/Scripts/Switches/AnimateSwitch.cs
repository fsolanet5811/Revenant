using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateSwitch : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void flipSwitchOn()
    {
        animator.SetBool("Flipped", true);
    }

    public void flipSwitchOff()
    {
        animator.SetBool("Flipped", false);
    }
}
