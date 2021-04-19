using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
    #region Singleton

    public static LevelManager Instance { get; }

    private LevelManager()
    {
        _initialEnemyStates = new Dictionary<Level, Dictionary<EnemyId, EnemyState>>();
        foreach (Level level in Enum.GetValues(typeof(Level)).Cast<Level>())
            _initialEnemyStates.Add(level, null);
    }

    static LevelManager()
    {
        Instance = new LevelManager();
        _levelNameMap = new Dictionary<Level, string>()
        {
            { Level.MainMenu, "Main_Menu" },
            { Level.IntroLevel, "Better Level Test" },
            { Level.Death, "dead" },
            { Level.FinalLevel, "Final Level" }
        };
    }

    #endregion

    private static readonly Dictionary<Level, string> _levelNameMap;

    private readonly Dictionary<Level, Dictionary<EnemyId, EnemyState>> _initialEnemyStates;

    public Level CurrentLevel { get; private set; }

    public Dictionary<EnemyId, EnemyState> CurrentInitialEnemyStates
    {
        get
        {
            return _initialEnemyStates[CurrentLevel];
        }
        private set
        {
            _initialEnemyStates[CurrentLevel] = value;
        }
    }

    public void GoToLevel(Level level)
    {
        // When we go to the main menu or die, the game will reset.
        if(level == Level.MainMenu || level == Level.Death)
        {
            ResetEnemyStates();
        }
        else
        {
            // Save the current state of the enemies.
            CurrentInitialEnemyStates = EnemiesManager.Instance.GetEnemyStates();
        }

        // Now change scene.
        // I beleive loading the scene will call the Awake functions, so we gotta make sure we are ready for it.
        CurrentLevel = level;

        // Reset the enemy manager to get rid of all the enemies from the previous scene.
        EnemiesManager.Instance.RemoveAllSpawnedEnemies();

        // Bring up the new scene.
        SceneManager.LoadScene(_levelNameMap[level]);
    }

    /// <summary>
    /// Removes all persistent enemy data.
    /// Effectively resets the enemy states to the initial state.
    /// </summary>
    private void ResetEnemyStates()
    {
        // The ToArray is so that the keys get enumerated over before we modify the dictionary.
        foreach (Level key in _initialEnemyStates.Keys.ToArray())
            _initialEnemyStates[key] = null;
    }
}
