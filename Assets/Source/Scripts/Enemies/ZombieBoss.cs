using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombieBoss : AlertableEnemy
{
    private bool _isAttackOnCooldown;

    [SerializeField]
    private WeightedTransform[] _waypoints;

    [SerializeField]
    private Transform[] _spawnWaypoints;

    public FloatData SpawnedEnemyAliveTimeSeconds;
    public FloatData AttackCooldownSeconds;

    protected override IMotionProvider GetUnalertMotionProvider()
    {
        return new RandomWaypointMotionProvider(_path, _destinationSetter, transform, _waypoints);
    }

    protected override void StartAttacking(PlayerController player)
    {
        _currentSpeed = 0;
        base.StartAttacking(player);
    }

    protected override void StopAttacking()
    {
        _currentSpeed = _baseSpeed;
        base.StopAttacking();
    }

    protected override IEnumerator AttackCoroutine(PlayerController player)
    {
        while(true)
        {
            // This guy has an op attack, so we need to give it a cooldown.
            // The cooldown should not reset when we stop and restart attacking.
            while (_isAttackOnCooldown)
                yield return null;

            _animator.AnimateAttack();
            yield return new WaitForSeconds(0.5f);

            SpawnEnemyInFront();

            yield return new WaitForSeconds(0.2f);
            _animator.StopAnimatingAttack();

            // Start the cooldown.
            // I moved the flag set out here because the while loop continues in this frame before the coroutine starts
            _isAttackOnCooldown = true;
            yield return StartCoroutine(AttackCooldownCoroutine()); 
        }
    }

    private void SpawnEnemyInFront()
    {
        // We need to see how far in front to spwan the enemy.
        // This will be done by straight up casting a ray in front of us and spawning an enemy in between us and the collider we detect, if any.
        Vector2 rayDir = DirectionUtilities.VectorFromDirection(CurrentDirection);

        const float rayDist = 2;
        float spawnDist = rayDist / 2;

        // I have to do the raycast in this janky way because my own collider gets in the way.
        // This way has the ray persist.
        RaycastHit2D[] hits = new RaycastHit2D[2];
        int numHits = Physics2D.Raycast(transform.position, rayDir, new ContactFilter2D(), hits, rayDist);
        if(numHits > 1)
        {
            // Adjust the spawn distance for the collider we hit.
            spawnDist *= hits[1].fraction;
        }
        else if(numHits == 1 && hits[0].fraction == 0)
        {
            // I know you are not supposed to check equality with floats, but the Unity docs said the fraction will ALWAYS be 0 when colliding with itself.
            // Makes sense.
            spawnDist *= hits[0].distance;
        }

        // We cannot spawn the enemy too close to us.
        if (spawnDist > 0.2)
        {
            SkullSwordsman enemy = EnemiesManager.Instance.SpawnEnemy<SkullSwordsman>((Vector2)transform.position + rayDir * spawnDist);

            // This enemy is going to track the same player.
            enemy.Target = Target;

            // We do not want these enemies to re-appear when reloading the game.
            enemy.IsPersistent = false;

            // Have this enemy wander around waypoints if some were provided.
            if (_waypoints != null && _waypoints.Length >= 2)
                enemy.Waypoints = _spawnWaypoints.OrderBy(wp => Vector2.Distance(wp.position, enemy.transform.position)).Take(2).ToArray();

            // To prevent to many enemies, we will kill this guy after some time.
            KillEnemyAfter(enemy, SpawnedEnemyAliveTimeSeconds);
        }
            
    }

    private void KillEnemyAfter(BasicEnemy enemy, float seconds)
    {
        // We are starting the coroutine on the enemy so that if the enemy gets destroyed, the coroutine will stop automatically.
        enemy.StartCoroutine(KillEnemyAfterCoroutine(enemy, seconds));
    }

    private IEnumerator KillEnemyAfterCoroutine(BasicEnemy enemy, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        enemy.Assasinate();
    }

    private IEnumerator AttackCooldownCoroutine()
    {
        yield return new WaitForSeconds(AttackCooldownSeconds);
        _isAttackOnCooldown = false;
    }
}
