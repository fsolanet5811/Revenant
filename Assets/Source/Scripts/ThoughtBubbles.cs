using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubbles : MonoBehaviour
{
   private GameObject textArea;
   private GameObject player;
   private Text introText;
   private Text seeEnemyText;
   private Text lockedText;
   private Text keycardText;
   private Text escapedText;
   private Text knifeText;
   private Text exploreText;

   private bool introFlag = true;
   private bool seeEnemyFlag = true;
   private bool lockedFlag = true;
   private bool keycardFlag = true;
   private bool escapedFlag = true;
   private bool knifeFlag = true;
   private bool exploreFlag = true;

    void Update()
    {
      FindObjects();
      RunText();
    }

    void FindObjects()
    {
      textArea = GameObject.Find("textArea");
      player = GameObject.Find("JoshuaSpatial");
      introText = GameObject.Find("introText").GetComponent<Text>();
      seeEnemyText = GameObject.Find("seeEnemyText").GetComponent<Text>();
      lockedText = GameObject.Find("lockedText").GetComponent<Text>();
      keycardText = GameObject.Find("keycardText").GetComponent<Text>();
      escapedText = GameObject.Find("escapedText").GetComponent<Text>();
      knifeText = GameObject.Find("knifeText").GetComponent<Text>();
      exploreText = GameObject.Find("exploreText").GetComponent<Text>();
    }

    void DisableAll()
    {
      textArea.GetComponent<Image>().enabled = false;
      introText.GetComponent<Text>().enabled = false;
      seeEnemyText.GetComponent<Text>().enabled = false;
      lockedText.GetComponent<Text>().enabled = false;
      keycardText.GetComponent<Text>().enabled = false;
      escapedText.GetComponent<Text>().enabled = false;
      knifeText.GetComponent<Text>().enabled = false;
      exploreText.GetComponent<Text>().enabled = false;
    }

    void RunText()
    {
      if (Input.GetKeyDown(KeyCode.E))
      {
         DisableAll();
         Time.timeScale = 1f;
      }

      if (introFlag)
      {
         introFlag = !introFlag;
         Time.timeScale = 0f;
         textArea.GetComponent<Image>().enabled = true;
         introText.GetComponent<Text>().enabled = true;
         StartCoroutine(ScrollText(introText));
      }

      if (seeEnemyFlag && player.transform.position.x < 5.4 && player.transform.position.y > -3.7)
      {
         seeEnemyFlag = !seeEnemyFlag;
         Time.timeScale = 0f;
         textArea.GetComponent<Image>().enabled = true;
         seeEnemyText.GetComponent<Text>().enabled = true;
         StartCoroutine(ScrollText(seeEnemyText));
      }

      if (keycardFlag && player.transform.position.x > 6.5 && player.transform.position.y > -2.7)
      {
         keycardFlag = !keycardFlag;
         lockedFlag = false;
         Time.timeScale = 0f;
         textArea.GetComponent<Image>().enabled = true;
         keycardText.GetComponent<Text>().enabled = true;
         StartCoroutine(ScrollText(keycardText));
      }

      if (lockedFlag && player.transform.position.x < 1.63)
      {
         lockedFlag = !lockedFlag;
         Time.timeScale = 0f;
         textArea.GetComponent<Image>().enabled = true;
         lockedText.GetComponent<Text>().enabled = true;
         StartCoroutine(ScrollText(lockedText));
      }

      if (escapedFlag && player.transform.position.x < 1)
      {
         escapedFlag = !escapedFlag;
         Time.timeScale = 0f;
         textArea.GetComponent<Image>().enabled = true;
         escapedText.GetComponent<Text>().enabled = true;
         StartCoroutine(ScrollText(escapedText));
      }

      if (exploreFlag && player.transform.position.y < -1.23 && player.transform.position.y >-2.15 && player.transform.position.x < -1.8)
      {
         exploreFlag = !exploreFlag;
         Time.timeScale = 0f;
         textArea.GetComponent<Image>().enabled = true;
         exploreText.GetComponent<Text>().enabled = true;
         StartCoroutine(ScrollText(exploreText));
      }

      if (knifeFlag && PlayerController.hasWeapon)
      {
         knifeFlag = !knifeFlag;
         Time.timeScale = 0f;
         textArea.GetComponent<Image>().enabled = true;
         knifeText.GetComponent<Text>().enabled = true;
         StartCoroutine(ScrollText(knifeText));
      }
    }

    IEnumerator ScrollText(Text text)
    {
      string str = text.text;
      text.text = "";
      int length = str.Length;
      int index = 0;

      while (index < length - 1)
      {
         yield return new WaitForSecondsRealtime(0.05f);
         text.text = text.text + str[index];
         index++;
      }
    }
}
