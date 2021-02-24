using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    private EnemyAnimator _animator;
    private Rigidbody2D _rigidBody;
    private IMotionProvider _motionProvider;
    private bool _isAttacking;

    [SerializeField]
    protected FloatData _baseSpeed;
    protected float _currentSpeed;

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

    protected virtual void Start()
    {
        _currentSpeed = _baseSpeed;
        _animator = new EnemyAnimator(GetComponent<Animator>());
        _rigidBody = GetComponent<Rigidbody2D>();
        _motionProvider = GetMotionProvider();
    }

    private void FixedUpdate()
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
                _animator.AnimateMove(direction);
            }
        }
    }

    protected virtual void StartAttacking(PlayerController player)
    {
        _isAttacking = true;
        _animator.AnimateAttack();
        _animator.AnimateDirection(player.transform.position - transform.position);
    }

    protected virtual void StopAttacking()
    {
        _isAttacking = false;
    }
}
