using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxInstantiator : MonoBehaviour
{
    private GameObject _hitBox;

    public FloatData HitBoxDistance;

    private void Start()
    {
        _hitBox = Resources.Load<GameObject>("Hit Box"); 
    }

    public GameObject Instantiate(float damage, int aliveFrames, Direction direction)
    {
        Vector2 translation = DirectionUtilities.VectorFromDirection(direction);

        // Scale the translation to make it spawn closer or farther.
        translation *= HitBoxDistance;
        GameObject spawned = Instantiate(_hitBox, transform.position + (Vector3)translation, Quaternion.identity);

        // Set the hitbox settings.
        TempHitBox hb = spawned.GetComponent<TempHitBox>();
        hb.Damage = damage;
        hb.AliveFrames = aliveFrames;

        return spawned;
    }
}
