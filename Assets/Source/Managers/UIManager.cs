using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   public static UIManager uiInstance;

   public GameObject pauseMenu;
   public static bool isGamePaused;

   void Awake()
   {
      CreateManager();
   }

   void Start()
   {
      pauseMenu.SetActive(false);
   }

   void Update()
   {
      if(Input.GetKeyDown(KeyCode.Escape))
      {
         if (isGamePaused)
            Resume();
         else
            Pause();
      }
   }

   public void Pause()
   {
      pauseMenu.SetActive(true);
      Time.timeScale = 0f;
      isGamePaused = true;
   }

   public void Resume()
   {
      pauseMenu.SetActive(false);
      Time.timeScale = 1f;
      isGamePaused = false;
   }

   public void LoadGame()
   {

   }

   public void ToMainMenu()
   {

   }

   public void QuitGame()
   {
      Application.Quit();
   }

   private void CreateManager()
   {
      if (uiInstance != null)
         Destroy(gameObject);
      else
      {
         uiInstance = this;
         DontDestroyOnLoad(gameObject);
      }
   }
}
