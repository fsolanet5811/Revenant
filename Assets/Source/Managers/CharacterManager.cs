/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
   public static CharacterManager characterInstance;

   public static int health;
   public bool speed;
   public static bool hasWeapon = false;
   public static GameObject weapon = null;

   void Awake()
   {
      health = 4;
      speed = false;
      CreateManager();
   }

   void Update()
   {
      UseItem();
   }

   private void UseItem()
   {
      if (Input.GetKeyDown(KeyCode.E) && !UIManager.isGamePaused)
      {
         GameObject buff = GetClosest();
         if (buff == null)
         return;
         if (Vector3.Distance(GameObject.Find("JoshuaSpatial").transform.position, buff.transform.position) < 2)
         {
            // health pack
            if (buff.name == "health" && health < 6)
            {
               health++;
               buff.SetActive(false);
            }
            // temporary speed boost
            else if (buff.name == "speed" && !speed)
            {
               buff.SetActive(false);
               speed = true;
               StartCoroutine(BuffTime());
            }
            // get weapon
            else if (buff.name == "knife" && !hasWeapon)
            {
               hasWeapon = true;
               weapon = buff;
               weapon.SetActive(false);
            }
         }
      }
   }

   IEnumerator BuffTime()
   {
      yield return new WaitForSeconds(20);
      speed = false;
   }

   private GameObject GetClosest()
   {
      GameObject [] buffs = GameObject.FindGameObjectsWithTag("buff");
      if (buffs.Length == 0)
         return null;
      GameObject nearest = null;
      float min = Mathf.Infinity;
      Vector3 currentPos = GameObject.Find("JoshuaSpatial").transform.position;
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
}*/
