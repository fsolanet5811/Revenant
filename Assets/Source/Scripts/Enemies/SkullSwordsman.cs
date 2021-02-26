using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HitBoxInstantiator))]
public class SkullSwordsman : BasicEnemy
{
    private HitBoxInstantiator _hitBoxInstantiator;

    [SerializeField]
    private FloatData _damage;

    protected override void Start()
    {
        _hitBoxInstantiator = GetComponent<HitBoxInstantiator>();
        base.Start();
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
    }

    protected override IEnumerator AttackCoroutine(PlayerController player)
    {
        while(true)
        {
            yield return new WaitForSeconds(0.2f);
            _hitBoxInstantiator.Instantiate(_damage, 2, _currentDirection);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
