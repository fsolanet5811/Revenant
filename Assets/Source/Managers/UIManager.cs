using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
   public static UIManager uiInstance;

   public static bool isGamePaused = false;
   private int health;
   public static GameObject pauseMenu;
   private GameObject noBuff, yesBuff, uiSpeed;
   public GameObject noWeapon, yesWeapon, uiKnife;
   public GameObject bar6, bar5, bar4, bar3, bar2, bar1, bar0;
   private int countdownTimer;
   public Text countdownText;

   void Awake()
   {
      CreateManager();
      FindBuffUI();
   }

   void Update()
   {
      health = PlayerController.playerHealth;

      if(Input.GetKeyDown(KeyCode.Escape))
      {
         FindPauseMenu();
         if (!isGamePaused)
            Pause();
         else
            Resume();
      }

      if (PlayerController.isUsingCDTimer)
      {
         StartCoroutine(SpeedBuffTimer());
         PlayerController.isUsingCDTimer = false;
      }

      SetHealthBar();
      WeaponUIUpdate();
   }

   private void FindPauseMenu()
   {
      if (pauseMenu != null)
         return;

      foreach(GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
         if (go.name == "pause menu")
         {
            pauseMenu = go;
            break;
         }
   }

   public void Pause()
   {
      pauseMenu.SetActive(true);
      Time.timeScale = 0f;
      isGamePaused = true;
   }

   public static void Resume()
   {
      pauseMenu.SetActive(false);
      Time.timeScale = 1f;
      isGamePaused = false;
   }

   public static void LoadGame()
   {

   }

   public static void ToMainMenu()
   {
      Resume();
      SceneManager.LoadScene("Main_Menu");
   }

   public static void QuitGame()
   {
      Application.Quit();
   }

   public void SetHealthBar()
   {
      FindHealthBars();

      if (health >= 5)
      {
         bar6.GetComponent<Image>().enabled = true;
         DisableBars(bar5, bar4, bar3, bar2, bar1, bar0);
      }
      else if (health >= -5)
      {
         bar5.GetComponent<Image>().enabled = true;
         DisableBars(bar6, bar4, bar3, bar2, bar1, bar0);
      }
      else if (health >= -15)
      {
         bar4.GetComponent<Image>().enabled = true;
         DisableBars(bar6, bar5, bar3, bar2, bar1, bar0);
      }
      else if (health >= -25)
      {
         bar3.GetComponent<Image>().enabled = true;
         DisableBars(bar6, bar5, bar4, bar2, bar1, bar0);
      }
      else if (health >= -35)
      {;
         bar2.GetComponent<Image>().enabled = true;
         DisableBars(bar6, bar5, bar4, bar3, bar1, bar0);
      }
      else if (health >= -45)
      {
         bar1.GetComponent<Image>().enabled = true;
         DisableBars(bar6, bar5, bar4, bar3, bar2, bar0);
      }
      else if (health <= -55)
      {
         bar0.GetComponent<Image>().enabled = true;
         DisableBars(bar6, bar5, bar4, bar3, bar2, bar1);
         PlayerController.hasWeapon = false;
            LevelManager.Instance.GoToLevel(Level.Death);
            //SceneManager.LoadScene("dead");
      }
   }

   private void FindHealthBars()
   {
      foreach(GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
      {
         if (go.name == "0")
            bar0 = go;
         if (go.name == "1")
            bar1 = go;
         if (go.name == "2")
            bar2 = go;
         if (go.name == "3")
            bar3 = go;
         if (go.name == "4")
            bar4 = go;
         if (go.name == "5")
            bar5 = go;
         if (go.name == "6")
            bar6 = go;
      }
   }

   // gameobject names do not correspond to names in the scene.
   // i'm using this naming scheme for ease of programming.
   private void DisableBars(GameObject bar1, GameObject bar2, GameObject bar3,
                              GameObject bar4, GameObject bar5, GameObject bar6)
   {
      bar1.GetComponent<Image>().enabled = false;
      bar2.GetComponent<Image>().enabled = false;
      bar3.GetComponent<Image>().enabled = false;
      bar4.GetComponent<Image>().enabled = false;
      bar5.GetComponent<Image>().enabled = false;
      bar6.GetComponent<Image>().enabled = false;
   }

   public void WeaponUIUpdate()
   {
      FindWeaponUI();

      if (PlayerController.hasWeapon)
      {
         noWeapon.SetActive(false);
         yesWeapon.SetActive(true);
         if (PlayerController.weapon.name.Contains("knife"))
            uiKnife.SetActive(true);
      }
      else
      {
         noWeapon.SetActive(true);
         yesWeapon.SetActive(false);
         uiKnife.SetActive(false);
      }
   }

   private void FindWeaponUI()
   {
      if (noWeapon != null)
         return;

      noWeapon = GameObject.Find("No Weapon");
      yesWeapon = GameObject.Find("Yes Weapon");
      uiKnife = GameObject.Find("uiKnife");
   }

   private void FindBuffUI()
   {
      noBuff = GameObject.Find("noBuff");
      yesBuff = GameObject.Find("yesBuff");
      uiSpeed = GameObject.Find("uiSpeed");

      if (noBuff != null)
      {
         yesBuff.SetActive(false);
         noBuff.SetActive(true);
         uiSpeed.SetActive(false);
      }
      else
         return;
   }

   IEnumerator SpeedBuffTimer()
   {
      countdownTimer = 20;

      noBuff.SetActive(false);
      yesBuff.SetActive(true);
      uiSpeed.SetActive(true);
      countdownText.GetComponent<Text>().enabled = true;

      while(countdownTimer > 0)
      {
         countdownText.text = countdownTimer.ToString();

         yield return new WaitForSeconds(1);

         countdownTimer--;
      }

      noBuff.SetActive(true);
      yesBuff.SetActive(false);
      uiSpeed.SetActive(false);
      countdownText.GetComponent<Text>().enabled = false;
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
