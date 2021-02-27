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
    }

    #endregion

    private readonly List<Enemy> _enemies;

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
        Object.Destroy(enemy.gameObject);
        _enemies.Remove(enemy);
    }
}
