using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollText : MonoBehaviour
{
   public static ScrollText scrollText;

   public GameObject player;
   public GameObject bubble1;
   //public GameObject bubble2;
   public int  index = 0;
   public Text bubble1Text;
   //public Text bubble2Text;
   private bool text1IsDone = false;
   private bool text1IsActivated = false;

   public string text1 = "Hey, where'd everyone go?\nPress E to continue...";
   /*public string [] text2 = {"H", "e", " ", "d", "o", "e", "s", "n", "\'",
                            "t", " ", "l", "o", "o", "k", " ", "o", "k",
                            "a", "y", ".", " ", "I", " ", "s", "h", "o",
                            "u", "l", "d", " ", "a", "v", "o", "i", "d",
                            " ", "h", "i", "m", " ", "f", "o", "r", " ",
                            "n", "o", "w", "."};*/

   void Start()
   {
      player = GameObject.Find("JoshuaSpatial");
      bubble1 = GameObject.Find("bubble1");
   }

    void Update()
    {
      bubble1Go();
    }

    private void bubble1Go()
    {
      if (!text1IsDone && !text1IsActivated)
      {
         Time.timeScale = 0f;
         text1IsActivated = true;
         bubble1.SetActive(true);
         StartCoroutine(StringDoer());
      }

      if (Input.GetKeyDown(KeyCode.E) && text1IsDone)
      {
         bubble1.SetActive(false);
         bubble1Text.GetComponent<Text>().enabled = false;
         Time.timeScale = 3f;
      }
   }

   IEnumerator StringDoer()
   {
      bubble1Text.GetComponent<Text>().enabled = true;

      while (index <= 35)
      {
         yield return new WaitForSeconds(0.25f);
         bubble1Text.text = bubble1Text.text + " " + text1[index];
         index++;
      }

      text1IsDone = true;
      index = 0;
   }
}
