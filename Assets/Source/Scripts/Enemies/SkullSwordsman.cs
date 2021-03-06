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

    protected override void Awake()
    {
        base.Awake();
        _hitBoxInstantiator = GetComponent<HitBoxInstantiator>();
    }

    protected override IEnumerator AttackCoroutine(PlayerController player)
    {
        while(true)
        {
            yield return new WaitForSeconds(0.2f);
            _hitBoxInstantiator.Instantiate(_damage, 20, CurrentDirection);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
