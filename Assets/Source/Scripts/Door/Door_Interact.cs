using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Interact : MonoBehaviour
{
    //lock plz
    [SerializeField] DoorAnimated door;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerController>() != null && door.isLocked() == false)
        {
            door.OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerController>() != null && door.isLocked() == false)
        {
            door.CloseDoor();
        }
    }
}
