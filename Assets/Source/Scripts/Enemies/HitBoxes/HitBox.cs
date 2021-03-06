using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class HitBox : MonoBehaviour
{
    public float Damage { get; set; }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
            OnPlayerCollided(player);
    }

    protected virtual void OnPlayerCollided(PlayerController player)
    {
        player.Damage((int)Damage);
    }
}
