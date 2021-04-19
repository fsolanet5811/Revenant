using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPath), typeof(AIDestinationSetter))]
public abstract class AlertableEnemy : Enemy
{
    private Coroutine _attackCoroutine;
    protected AlertZone _alertZone;
    protected FollowerMotionProvider _alertMotionProvider;
    protected AIPath _path;
    protected AIDestinationSetter _destinationSetter;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Vector2Data _alertTriggerDimensions;

    public Transform Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
        }
    }

    public bool IsAlerted
    {
        get
        {
            return _alertZone.IsAlerted;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _alertZone = GetComponentInChildren<AlertZone>();
        _path = GetComponent<AIPath>();
        _destinationSetter = GetComponent<AIDestinationSetter>();
        
    }

    protected override void Start()
    {
        _alertMotionProvider = new FollowerMotionProvider(_path, _destinationSetter, transform, Target);
        _alertZone.AlertTarget = _target;
        _alertZone.SetTriggerDimensions(_alertTriggerDimensions);

        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // give the alert zone our current direction.
        _alertZone.CurrentDirection = CurrentDirection;
    }

    #region Attack

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

        // Whenever we start attacking, we ensure that the enemy is in the alert state.
        _alertZone.ForceAlert();
        Attack(player);
    }

    protected override void StopAttacking()
    {
        base.StopAttacking();
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }

    private void Attack(PlayerController player)
    {
        _attackCoroutine ??= StartCoroutine(AttackCoroutine(player));
    }

    protected abstract IEnumerator AttackCoroutine(PlayerController player);

    #endregion

    protected abstract IMotionProvider GetUnalertMotionProvider();

    protected override IMotionProvider GetMotionProvider()
    {
        return new AlertableMotionProvider(_alertZone, _alertMotionProvider, GetUnalertMotionProvider());
    }
}
