using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertZone : MonoBehaviour
{
    private PolygonCollider2D _alertTrigger;
    private Collider2D _alertRelease;
    private Direction _currentDirection;

    public bool IsAlerted { get; private set; }

    public Transform AlertTarget { get; set; }

    public Direction CurrentDirection
    {
        get
        {
            return _currentDirection;
        }
        set
        {
            // No need to perform the calculation again if we haven't changed.
            if(_currentDirection != value)
            {
                _currentDirection = value;
                DirectionUtilities.RotateTransformTowardsDirection(_alertTrigger.transform, value);
            }
        }
    }

    private void Awake()
    {
        _alertTrigger = transform.GetChild(0).GetComponent<PolygonCollider2D>();
        _alertRelease = transform.GetChild(1).GetComponent<Collider2D>();
    }

    void Update()
    {
        if (AlertTarget != null)
            CheckAlert();
    }

    public void ForceAlert()
    {
        IsAlerted = true;
    }

    public void SetTriggerDimensions(Vector2 dimensions)
    {
        SetTriggerDimensions(dimensions.x, dimensions.y);
    }

    public void SetTriggerDimensions(float width, float height)
    {
        _alertTrigger.points = new Vector2[]
        {
            Vector2.zero,
            new Vector2(height , width / 2),
            new Vector2(height, -width / 2)
        };
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
