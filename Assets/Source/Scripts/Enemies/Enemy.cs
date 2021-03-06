using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    
    protected bool _isMovingEnabled;
    protected AttackZone _attackZone;
    private EnemyAnimator _animator;
    private Rigidbody2D _rigidBody;
    private IMotionProvider _motionProvider;
    private bool _isAttacking;
    private Direction _currentDirection;

    [SerializeField]
    protected FloatData _baseSpeed;
    protected float _currentSpeed;

    [SerializeField]
    protected FloatData _baseHealth;

    [SerializeField]
    private Vector2Data _attackZoneDimensions;

    protected Direction CurrentDirection
    {
        get
        {
            return _currentDirection;
        }
        private set
        {
            _currentDirection = value;
            _attackZone.CurrentDirection = value;
        }
    }

    public float CurrentHealth { get; protected set; }


    #region Collision

    protected abstract void OnPlayerEnteredAttackZone(PlayerController player);

    protected abstract void OnPlayerExitedAttackZone(PlayerController player);

    public void OnGameObjectEnteredAttackZone(GameObject gameObject)
    {
        // See if this was the player.
        PlayerController player = gameObject.GetComponent<PlayerController>();
        if (player)
            OnPlayerEnteredAttackZone(player);
    }

    public void OnGameObjectExitedAttackZone(GameObject gameObject)
    {
        // See if this was the player.
        PlayerController player = gameObject.GetComponent<PlayerController>();
        if (player)
            OnPlayerExitedAttackZone(player);
    }

    #endregion

    #region Unity

    protected virtual void Awake()
    {
        _animator = new EnemyAnimator(GetComponent<Animator>());
        _attackZone = GetComponentInChildren<AttackZone>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        CurrentHealth = _baseHealth;
        _currentSpeed = _baseSpeed;
        _motionProvider = GetMotionProvider();
        _attackZone.SetDimensions(_attackZoneDimensions);
        EnemiesManager.Instance.AddSpawnedEnemy(this);
    }

    protected virtual void FixedUpdate()
    {
        Vector2 moveDireciton = Move();
        Animate(moveDireciton);
    }

    #endregion

    #region Motion

    /// <summary>
    /// Called once in MonoBehaviour's Start method.
    /// </summary>
    /// <returns>
    /// An implementation of the motion provider for this enemy type.
    /// </returns>
    protected abstract IMotionProvider GetMotionProvider();

    private Vector2 Move()
    {
        // Get the motion from our provider and normalize it to keep a consistent speed.
        Vector2 motion = _motionProvider.GetMotion();
        motion.Normalize();

        // This should get us moving!
        _rigidBody.velocity = motion * _currentSpeed;
        return motion;
    }

    #endregion

    public void Damage(float damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        if (CurrentHealth == 0)
            OnKilled();
    }

    private void Animate(Vector2 direction)
    {
        if(!_isAttacking)
        {
            if(direction == Vector2.zero)
            {
                _animator.AnimateIdle();
            }
            else
            {
                CurrentDirection = DirectionUtilities.DirectionFromVector(direction);
                _animator.AnimateMove(CurrentDirection);
            }
        }
    }

    protected virtual void StartAttacking(PlayerController player)
    {
        _isAttacking = true;
        _animator.AnimateAttack();

        // Attacking will also change our direction.
        CurrentDirection = DirectionUtilities.DirectionFromVector(player.transform.position - transform.position);
        _animator.AnimateDirection(CurrentDirection);
    }

    protected virtual void StopAttacking()
    {
        _isAttacking = false;
    }

    protected virtual void OnKilled()
    {
        EnemiesManager.Instance.DespawnEnemy(this);
    }
}
