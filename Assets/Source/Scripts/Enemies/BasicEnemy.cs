using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIPath), typeof(AIDestinationSetter))]
public abstract class BasicEnemy : Enemy
{
    protected FollowerMotionProvider _alertMotionProvider;
    protected WaypointMotionProvider _unalertMotionProvider;
    protected AlertZone _alertZone;
    protected AssasinationZone _assasinationZone;
    private Coroutine _attackCoroutine;

    [SerializeField]
    private Vector2Data _alertTriggerDimensions;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Transform[] _waypoints;

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
        _assasinationZone = GetComponentInChildren<AssasinationZone>();
    }

    protected override void Start()
    {
        _alertZone.AlertTarget = _target;
        _alertZone.SetTriggerDimensions(_alertTriggerDimensions);

        // Center the assaination zone around our physical hitbox.
        _assasinationZone.Place(_physicalHitBox.offset);

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
        if(_attackCoroutine != null)
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

    public void Assasinate()
    {
        Damage(CurrentHealth);
    }

    protected override IMotionProvider GetMotionProvider()
    {
        AIPath path = GetComponent<AIPath>();
        AIDestinationSetter destinationSetter = GetComponent<AIDestinationSetter>();
        _alertMotionProvider = new FollowerMotionProvider(path, destinationSetter, transform, _target);
        _unalertMotionProvider = new CircularWaypointMotionProvider(path, destinationSetter, transform, _waypoints);
        return new AlertableMotionProvider(_alertZone, _alertMotionProvider, _unalertMotionProvider);
    }
}
