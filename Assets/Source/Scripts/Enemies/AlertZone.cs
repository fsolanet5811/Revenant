using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertZone : MonoBehaviour
{
    private Collider2D _alertTrigger;
    private Collider2D _alertRelease;

    public bool IsAlerted { get; private set; }

    public Transform AlertTarget { get; set; }

    void Start()
    {
        _alertTrigger = transform.GetChild(0).GetComponent<Collider2D>();
        _alertRelease = transform.GetChild(1).GetComponent<Collider2D>();
    }

    void Update()
    {
        if (AlertTarget != null)
            CheckAlert();
    }

    private void CheckAlert()
    {
        if(_alertTrigger.bounds.Contains(AlertTarget.position))
        {
            // They are inside the trigger zone. This means we are for sure alerted.
            IsAlerted = true;
        }
        else if(!_alertRelease.bounds.Contains(AlertTarget.position))
        {
            IsAlerted = false;
        }
    }

}
