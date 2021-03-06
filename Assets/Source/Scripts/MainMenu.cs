using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGame()
    {
      SceneManager.LoadScene("Better Level Test");
    }

    public void Quit()
    {
      Application.Quit();
    }

    public void ToMainMenu()
    {
      SceneManager.LoadScene("Main_Menu");
   }
}
