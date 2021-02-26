using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Animator), typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    private ProjectileAnimator _animator;
    private Rigidbody2D _rigidBody;
    private IMotionProvider _motionProvider;
    private bool _isExploding;

    public GameObject SpawningObject { get; set; }

    [SerializeField]
    protected FloatData _speed;

    [SerializeField]
    protected FloatData _damage;

    protected virtual void Start()
    {
        _animator = new ProjectileAnimator(GetComponent<Animator>());
        _rigidBody = GetComponent<Rigidbody2D>();
        _motionProvider = GetMotionProvider();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Trigger colliders are safe!
        // And so is the spawning object!
        if(!collision.isTrigger && collision.gameObject != SpawningObject)
        {
            // If the other object is an enemy, we will damage it.
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy)
                enemy.Damage(_damage);

            Explode();
        }
    }

    private void Move()
    {
        if(_isExploding)
        {
            _rigidBody.velocity = Vector2.zero;
        }
        else
        {
            Vector2 motion = _motionProvider.GetMotion();
            if (motion != Vector2.zero)
            {
                float angle = Mathf.Atan2(motion.y, motion.x) - Mathf.Atan2(transform.right.y, transform.right.x);
                transform.Rotate(new Vector3(0, 0, angle * 180 / Mathf.PI));

                // Because we rotated the transform, the velocity direction is always the right axis.
                _rigidBody.velocity = transform.right * _speed;
            }
            else
            {
                _rigidBody.velocity = Vector2.zero;
            }
        }
    }

    protected abstract IMotionProvider GetMotionProvider();

    private void Explode()
    {
        StartCoroutine(ExplodeCoroutine());
    }

    private IEnumerator ExplodeCoroutine()
    {
        _isExploding = true;
        _animator.AnimateExplosion();
        yield return new WaitForSeconds(0.87f);
        Destroy(gameObject);
    }
}
