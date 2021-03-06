using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileInstantiator))]
public class SkullMage : BasicEnemy
{
    [SerializeField]
    private GameObject _projectile;
    private ProjectileInstantiator _projectileInstantiator;

    protected override void Awake()
    {
        base.Awake();
        _projectileInstantiator = GetComponent<ProjectileInstantiator>();
    }

    protected override void StartAttacking(PlayerController player)
    {
        _currentSpeed = 0;
        base.StartAttacking(player);
    }

    protected override void StopAttacking()
    {
        _currentSpeed = _baseSpeed;
        base.StopAttacking();
    }

    protected override IEnumerator AttackCoroutine(PlayerController player)
    {
        while(true)
        {
            // The animation will take 1/3 of a second.
            Vector2 direction = player.transform.position - transform.position;
            yield return new WaitForSeconds(0.33f);

            // Now we can attack.
            _projectileInstantiator.Instantiate(_projectile, direction);
        }
    }
}
