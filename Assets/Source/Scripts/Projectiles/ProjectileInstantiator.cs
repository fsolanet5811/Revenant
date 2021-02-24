using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInstantiator : MonoBehaviour
{
    private static readonly float RADIANS_TO_DEGREES = 180 / Mathf.PI;

    public GameObject Instantiate(GameObject original)
    {
        return Instantiate(original, transform.right);
    }

    public GameObject Instantiate(GameObject original, Vector2 direction)
    {
        GameObject inst = Instantiate(original, transform.position, Quaternion.Euler(0, 0, 0));
        
        // This prevents the projectile from exploding on the object that spawned it.
        Projectile instProj = inst.GetComponent<Projectile>();
        instProj.SpawningObject = gameObject;

        // This sends the projectile in the initial trajectory.
        inst.transform.Rotate(0, 0, Mathf.Atan2(direction.y, direction.x) * RADIANS_TO_DEGREES);
        return inst;
    }
}
