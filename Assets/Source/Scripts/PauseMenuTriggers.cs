using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuTriggers : MonoBehaviour
{
    public void Resume()
    {
      UIManager.Resume();
   }

   public void ToMainMenu()
   {
      UIManager.ToMainMenu();
   }

   public void Quit()
   {
      UIManager.QuitGame();
   }
}
