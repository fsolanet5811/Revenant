using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HitBoxInstantiator))]
public class Cyclops : BasicEnemy
{
    private HitBoxInstantiator _hitBoxInstantiator;

    [SerializeField]
    private FloatData _damage;

    protected override void Awake()
    {
        _hitBoxInstantiator = GetComponent<HitBoxInstantiator>();
        base.Awake();
    }

    protected override IEnumerator AttackCoroutine(PlayerController player)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            _hitBoxInstantiator.Instantiate(_damage, 2, CurrentDirection);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
