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
    }

    #endregion

    private readonly List<Enemy> _enemies;
    private readonly List<BasicEnemy> _assasinatableEnemies;

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
    /// Checks to see if any of the enemies are alerted.
    /// </summary>
    /// <returns>
    /// True if at least one enemy is alerted, false otherwise.
    /// </returns>
    public bool IsEnemyAlerted()
    {
        return _enemies.Exists(e => e is BasicEnemy be && be.IsAlerted);
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
        // You obviously cannot assasinate an enemy if it is despawned.
        if (enemy is BasicEnemy be)
            RemoveAssasinatableEnemy(be);

        Object.Destroy(enemy.gameObject);
        _enemies.Remove(enemy);
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

    private void CleanAssasinatableEnemies()
    {
        // Remove all the enemies that are alert.
        _assasinatableEnemies.RemoveAll(be => be.IsAlerted);
    }
}
