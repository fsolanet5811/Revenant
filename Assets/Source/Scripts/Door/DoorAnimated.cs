using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimated : MonoBehaviour
{
    private Animator animator;
    [SerializeField] bool locked;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        animator.SetBool("Open", true);
    }

    public void CloseDoor()
    {
        animator.SetBool("Open", false);
    }

    public bool isLocked()
    {
        if (locked == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void unlockDoor()
    {
        locked = false;
    }
}
