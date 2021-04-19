using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesManager
{
    #region Singleton

    public static EnemiesManager Instance { get; }

    static EnemiesManager()
    {
        Instance = new EnemiesManager();
    }

    private EnemiesManager() 
    {
        _enemies = new List<Enemy>();
        _assasinatableEnemies = new List<BasicEnemy>();
        _enemySpawner = new EnemySpawner();
        _despawnedEnemies = new List<EnemyId>();
    }

    #endregion

    private readonly List<Enemy> _enemies;
    private readonly List<BasicEnemy> _assasinatableEnemies;
    private readonly EnemySpawner _enemySpawner;
    private readonly List<EnemyId> _despawnedEnemies;

    /// <summary>
    /// Adds an enemy to the list of enemies that are spawned into the game.
    /// This should be called in the startup of every enemy.
    /// </summary>
    /// <param name="enemy">
    /// The enemy that was spawned
    /// </param>
    public void AddSpawnedEnemy(Enemy enemy)
    {
        _enemies.Add(enemy);
    }

    /// <summary>
    /// Spawns an enemy into the game.
    /// </summary>
    /// <typeparam name="T">
    /// The type of enemy to spawn into the game.
    /// </typeparam>
    /// <param name="position">
    /// Where the enemy should be spawned.
    /// </param>
    /// <returns>
    /// The spawned enemy.
    /// </returns>
    public T SpawnEnemy<T>(Vector2 position) where T : Enemy
    {
        // Spawn in the enemy.
        T enemy = _enemySpawner.Spawn<T>(position);

        // Keep track of this enemy.
        AddSpawnedEnemy(enemy);

        return enemy;
    }

    /// <summary>
    /// Checks to see if any of the enemies are alerted.
    /// </summary>
    /// <returns>
    /// True if at least one enemy is alerted, false otherwise.
    /// </returns>
    public bool IsEnemyAlerted()
    {
        return _enemies.Exists(e => e is AlertableEnemy ae && ae.IsAlerted);
    }

    /// <summary>
    /// Removes the enmy from the game.
    /// This destroys the gameobject and stops keeping track of the enemy.
    /// </summary>
    /// <param name="enemy">
    /// The enemy to remove.
    /// </param>
    public void DespawnEnemy(Enemy enemy)
    {
        RemovedSpawnedEnemy(enemy);

        // Keep track that we despawned this enemy.
        // Because it was removed from the list in the above call, it's state will not be captured.
        // We actually care about despawned enemies that are persistent.
        if(enemy.IsPersistent)
            _despawnedEnemies.Add(enemy.Id);

        Object.Destroy(enemy.gameObject);
    }

    /// <summary>
    /// Removes an enemy from the list of enemies in the game.
    /// Does not actually despawn the enemy.
    /// </summary>
    /// <param name="enemy">
    /// The enemy to remove from the list.
    /// </param>
    public void RemovedSpawnedEnemy(Enemy enemy)
    {
        // You cannot assasinate an enemy if we are not keeping track of it.
        if (enemy is BasicEnemy be)
            RemoveAssasinatableEnemy(be);

        _enemies.Remove(enemy);
    }

    /// <summary>
    /// Despawns all enemies in the list of enemies.
    /// </summary>
    public void DespawnAllEnemies()
    {
        while (_enemies.Any())
            DespawnEnemy(_enemies.First());
    }

    /// <summary>
    /// removes all enemies from the list of enemies in the game.
    /// Does not despawn any of them.
    /// </summary>
    public void RemoveAllSpawnedEnemies()
    {
        while (_enemies.Any())
            RemovedSpawnedEnemy(_enemies.First());
    }

    /// <summary>
    /// Adds an enemy to the list of enemies that can be assasinated.
    /// </summary>
    /// <param name="enemy">
    /// The enemy that can be assasinated.
    /// </param>
    public void AddAssasinatableEnemy(BasicEnemy enemy)
    {
        if (!_assasinatableEnemies.Contains(enemy))
            _assasinatableEnemies.Add(enemy);
    }

    /// <summary>
    /// Removes an enemy from the list of enemies that can be assasinated.
    /// </summary>
    /// <param name="enemy">
    /// The enemy that can no longer be assasinated.
    /// </param>
    public void RemoveAssasinatableEnemy(BasicEnemy enemy)
    {
        _assasinatableEnemies.Remove(enemy);
    }

    /// <summary>
    /// Attempts to get an enemy that can be assasinated.
    /// </summary>
    /// <returns>
    /// An enemy that can be assasinated if one is present.
    /// Null if no enemies can be assasinated.
    /// </returns>
    public BasicEnemy TryGetAssasinatableEnemy()
    {
        // Make sure we do not give them an enemy that is alerted.
        CleanAssasinatableEnemies();
        return _assasinatableEnemies.FirstOrDefault();
    }

    /// <summary>
    /// Gets a current snapshot of all the enemy states.
    /// </summary>
    /// <returns>
    /// The states of all the enemys (spawned and despawned).
    /// </returns>
    public Dictionary<EnemyId, EnemyState> GetEnemyStates()
    {
        Dictionary<EnemyId, EnemyState> states = new Dictionary<EnemyId, EnemyState>();

        // Load in all the despawned enemies.
        foreach (EnemyId id in _despawnedEnemies)
            states.Set(id, EnemyState.Despawned());

        // Now grab the states from all of our enemies that are persistent.
        foreach (Enemy enemy in _enemies.Where(e => e.IsPersistent))
            states.Set(enemy.Id, enemy.GetState());

        return states;
    }

    private void CleanAssasinatableEnemies()
    {
        // Remove all the enemies that are alert.
        _assasinatableEnemies.RemoveAll(be => be.IsAlerted);
    }
}
