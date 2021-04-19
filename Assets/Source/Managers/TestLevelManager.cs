//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class TestLevelManager
//{
//    #region Singleton

//    public static TestLevelManager Instance { get; }

//    private TestLevelManager()
//    {
//        _initialEnemyStates = new Dictionary<Level, Dictionary<EnemyId, EnemyState>>();
//        foreach (Level level in Enum.GetValues(typeof(Level)).Cast<Level>())
//            _initialEnemyStates.Add(level, null);
//    }

//    static TestLevelManager()
//    {
//        Instance = new TestLevelManager();
//        _levelNameMap = new Dictionary<Level, string>()
//        {
//            { Level.MainMenu, "EnemiesTestScene" },
//            { Level.Duplicate, "EnemiesTestScene Duplicate" }
//        };
//    }

//    #endregion

//    private static readonly Dictionary<Level, string> _levelNameMap;

//    private readonly Dictionary<Level, Dictionary<EnemyId, EnemyState>> _initialEnemyStates;

//    public Level CurrentLevel { get; private set; }

//    public Dictionary<EnemyId, EnemyState> CurrentInitialEnemyStates
//    {
//        get
//        {
//            return _initialEnemyStates[CurrentLevel];
//        }
//        private set
//        {
//            _initialEnemyStates[CurrentLevel] = value;
//        }
//    }

//    public void GoToLevel(Level level)
//    {
//        // Save the current state of the enemies.
//        CurrentInitialEnemyStates = EnemiesManager.Instance.GetEnemyStates();

//        // Now change scene.
//        // I beleive loading the scene will call the Awake functions, so we gotta make sure we are ready for it.
//        CurrentLevel = level;
//        EnemiesManager.Instance.RemoveAllSpawnedEnemies();
//        SceneManager.LoadScene(_levelNameMap[level]);
//    }
//}
