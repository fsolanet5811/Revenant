using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxInstantiator : MonoBehaviour
{
    private GameObject _hitBox;

    public float HitBoxDistance = 1;

    private void Start()
    {
        _hitBox = Resources.Load<GameObject>("Hit Box"); 
    }

    public GameObject Instantiate(float damage, int aliveFrames, Direction direction)
    {
        var translation = direction switch
        {
            Direction.Down => new Vector2(0, -1),
            Direction.Up => new Vector2(0, 1),
            Direction.Right => new Vector2(1, 0),
            Direction.Left => new Vector2(-1, 0),
            _ => throw new System.NotImplementedException($"Unimplemented direction {direction}."),
        };

        // Scale the translation to make it spawn closer or farther.
        translation *= HitBoxDistance;
        GameObject spawned = Instantiate(_hitBox, transform.position + (Vector3)translation, Quaternion.identity);

        // Set the hitbox settings.
        HitBox hb = spawned.GetComponent<HitBox>();
        hb.Damage = damage;
        hb.AliveFrames = aliveFrames;

        return spawned;
    }
}
