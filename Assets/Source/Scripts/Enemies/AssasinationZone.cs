using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AssasinationZone : MonoBehaviour
{
    private BasicEnemy _parent;
    private Collider2D _collider;

    private void Awake()
    {
        _parent = GetComponentInParent<BasicEnemy>();
        _collider = GetComponent<Collider2D>();
    }

    public void Place(Vector2 center)
    {
        _collider.offset = center;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // We can be assasinated if we are not alerted and the player just entered this zone.
        // Technically, if the enemy becomes unalerted while we are still in the zone (have never left), the enemy will never get added to the assasination list.
        // This should be fine since the alert release zone will always be much larger than assasination zone.
        // By not taking this into account, we allow the assasinate system to be fully event-based and not require checks every frame, or a linkage of the alert and assasinte system.
        if (!_parent.IsAlerted && CollidedWithPlayer(collision))
            EnemiesManager.Instance.AddAssasinatableEnemy(_parent);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(CollidedWithPlayer(collision))
        {
            // We can no longer be assasinated!
            EnemiesManager.Instance.RemoveAssasinatableEnemy(_parent);
        }
    }

    private static bool CollidedWithPlayer(Collider2D collision)
    {
        return collision.gameObject.GetComponent<PlayerController>();
    }
}
