using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectileInstantiator))]
public class SkullHead : BasicEnemy
{
    private ProjectileInstantiator _projectileInstantiator;

    [SerializeField]
    private GameObject _projectile;

    protected override void Start()
    {
        base.Start();
        _projectileInstantiator = GetComponent<ProjectileInstantiator>();
    }

    protected override void OnPlayerEnteredAttackZone(PlayerController player)
    {
        StartAttacking(player);
    }

    protected override void OnPlayerExitedAttackZone(PlayerController player)
    {
        StopAttacking();
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
        while (true)
        {
            // The animation will have a cooldown.
            Vector2 direction = player.transform.position - transform.position;
            _projectileInstantiator.Instantiate(_projectile, direction);
            yield return new WaitForSeconds(0.8f);
        }
    }
}
