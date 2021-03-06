using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackZone : MonoBehaviour
{
    private Direction _currentDirection;
    private Enemy _parent;
    private PolygonCollider2D _zone;

    public Direction CurrentDirection
    {
        get
        {
            return _currentDirection;
        }
        set
        {
            // No need to perform the calculation again if we haven't changed.
            if (_currentDirection != value)
            {
                _currentDirection = value;
                DirectionUtilities.RotateTransformTowardsDirection(transform, value);
            }
        }
    }

    private void Awake()
    {
        _zone = GetComponent<PolygonCollider2D>();
    }

    void Start()
    {
        _parent = gameObject.GetComponentInParent<Enemy>();
    }

    public void SetDimensions(Vector2 dimensions)
    {
        SetDimensions(dimensions.x, dimensions.y);
    }

    public void SetDimensions(float width, float height)
    {
        _zone.points = new Vector2[]
        {
            Vector2.zero,
            new Vector2(height , width / 2),
            new Vector2(height, -width / 2)
        };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _parent.OnGameObjectEnteredAttackZone(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _parent.OnGameObjectExitedAttackZone(collision.gameObject);
    }
}
