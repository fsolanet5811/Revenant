using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertZone : MonoBehaviour
{
    private PolygonCollider2D _alertTrigger;
    private Collider2D _alertRelease;
    private Direction _currentDirection;

    public LayerMask ObstacleLayer;

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
            // We are alerted if we are in the trigger zone and can cast to the target.
            IsAlerted = CanCastToTarget();
        }
        else if(_alertRelease.bounds.Contains(AlertTarget.position))
        {
            // No need for an unnecessary calculation.
            if(IsAlerted && !CanCastToTarget())
            {
                IsAlerted = false;
            }
        }
        else
        {
            // Outside the release zone. We are definitely not alerted.
            IsAlerted = false;
        }
    }

    private bool CanCastToTarget()
    {
        // This is a decent way of accounting for obstacles in the way of the line of sight.
        Vector3 towardsTarget = AlertTarget.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, towardsTarget, towardsTarget.magnitude, ObstacleLayer);

        // We can cast to the target if there is no collider between us.
        return !hit.collider;
    }
}
