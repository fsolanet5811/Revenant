using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesManager
{
    #region Singleton

    private static readonly EnemiesManager _instance;

    public static EnemiesManager Instance
    {
        get
        {
            return Instance;
        }
    }

    static EnemiesManager()
    {
        _instance = new EnemiesManager();
    }

    private EnemiesManager() 
    {
        _enemies = new List<Enemy>();
    }

    #endregion

    private readonly List<Enemy> _enemies;

    /// <summary>
    /// Brings the enemy manager to an initial state.
    /// Should be called once at the start of the game.
    /// </summary>
    /// <param name="enemies">
    /// The enemies that are staticly placed/spawned at the start of the game.
    /// </param>
    public void Initialize(IEnumerable<Enemy> enemies)
    {
        _enemies.Clear();
        _enemies.AddRange(enemies);
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
        Object.Destroy(enemy);
        _enemies.Remove(enemy);
    }
}
