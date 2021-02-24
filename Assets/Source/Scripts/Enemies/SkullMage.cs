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
    private Coroutine _attackCoroutine;

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
        base.StartAttacking(player);
        Attack(player);
    }

    protected override void StopAttacking()
    {
        base.StopAttacking();
        StopCoroutine(_attackCoroutine);
        _attackCoroutine = null;
    }

    private void Attack(PlayerController player)
    {
        _attackCoroutine ??= StartCoroutine(AttackCoroutine(player));
    }

    private IEnumerator AttackCoroutine(PlayerController player)
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
