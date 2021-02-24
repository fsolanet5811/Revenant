using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackZone : MonoBehaviour
{
    private Enemy _parent;

    void Start()
    {
        _parent = gameObject.GetComponentInParent<Enemy>();
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
