using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
   public static CharacterManager characterInstance;

   public int health = 5;
   public int speed = 1;
   public bool isAlive = true;
   public GameObject itemBuff = null;
   public GameObject characterWeapon = null;

   void Awake()
   {
      CreateManager();
   }

   void Update()
   {
      UseItem();
   }

   private void UseItem()
   {
      GameObject buff = GetClosest();
      if (Vector3.Distance(GameObject.Find("box").transform.position, buff.transform.position) < 2)
      {
         if (Input.GetKeyDown(KeyCode.W) && !UIManager.isGamePaused)
         {
            health++;
            buff.SetActive(false);
            Debug.Log(health);
         }
      }
   }

   private GameObject GetClosest()
   {
      GameObject [] buffs = GameObject.FindGameObjectsWithTag("buff");
      GameObject nearest = null;
      float min = Mathf.Infinity;
      Vector3 currentPos = GameObject.Find("box").transform.position;
      foreach (GameObject g in buffs)
      {
         float distance = Vector3.Distance(g.transform.position, currentPos);
         if (distance < min)
         {
            nearest = g;
            min = distance;
         }
      }
      return nearest;
   }

   private void CreateManager()
   {
      if (characterInstance != null)
         Destroy(gameObject);
      else
      {
         characterInstance = this;
         DontDestroyOnLoad(gameObject);
      }
   }
}
