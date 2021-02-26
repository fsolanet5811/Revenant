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
