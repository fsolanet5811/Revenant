using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update :p
    [SerializeField] private Field_of_View fieldOfView;
    private float baseSpeed = 1.5f;
    //may or may not have speed potions, delete baseSpeed and assign value to currentSpeed if not.
    [SerializeField] float currentSpeed;
    [SerializeField] public static int playerHealth;

    public static bool hasWeapon = false;
    public static bool isUsingCDTimer = false;
    public static GameObject weapon = null;
    private GameObject player;
    private GameObject useItemText;
    private GameObject killText;

    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private AudioClip oo;
    [SerializeField] private AudioClip stab;
    private AudioSource damageSound;
    private AudioSource attackSound;
    private BasicEnemy enemy;

    //making moveDirection a non-local variable allows the code to get its magnitude in Animate()
    private Vector3 moveDirection;
    //shows the direction player is facing
    private float lookVertical = -1, lookHorizontal = 0;
    //tell the fov cone which angle to point in based on player movement
    private Vector3 coneDirection;

    //this bool tells what the orignal direction the player was moving before the new Update() is called.
    //private bool wasMovingHorizontal = false;//part of old code that only allowed 4 direction movement

    void Start()
    {
        damageSound = gameObject.AddComponent<AudioSource>();
        damageSound.clip = oo;
        damageSound.Stop();
        attackSound = gameObject.AddComponent<AudioSource>();
        attackSound.clip = stab;
        attackSound.Stop();
        rb = GetComponent<Rigidbody2D>();
        coneDirection = new Vector3(0, -1, 0);
        fieldOfView.SetAimDirection(coneDirection);
        playerHealth = 5;

        currentSpeed = baseSpeed;
        player = GameObject.Find("JoshuaSpatial");
        useItemText = GameObject.Find("UseItemText");
        killText = GameObject.Find("KillText");
        if (useItemText != null)
            useItemText.GetComponent<Text>().enabled = false;
        if (killText != null)
            killText.GetComponent<Text>().enabled = false;

        Physics2D.gravity = Vector2.zero; //This keeps the playing from falling directly down the screen

        animator = GetComponent<Animator>();
        /*DontDestroyOnLoad(gameObject);
        transform.position = transform.position;
        SceneManager.sceneLoaded += OnSceneLoaded;*/ //Code notes for class

    }

    /*void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }*/
    // Update is called once per frame
    void Update()
    {
        Move();
        //this will tell the fov what direction the player is facing
        fieldOfView.SetAimDirection(coneDirection);
        //this will tell fov where to draw the vision cone which will be on the player position
        fieldOfView.SetOrigin(this.transform.position);
        Animate();
        UseItem();
        if (Input.GetMouseButtonDown(0))
        {
            if(hasWeapon) Assassinate(enemy);
        }
    }

    void Move()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        //bool isMovingHorizontal = (Input.GetAxisRaw("Horizontal") != 0);
        //bool isMovingVertical = (Input.GetAxisRaw("Vertical") != 0);

        //if both keys are being pressed, don't update  wasMovingHorizontal otherwise it will move diagonally in a messed up way.
        //Reason is, once the character moves a tiny bit in one direction then Update() is called changing the variable rapidly switching isMovingHorizontal from true to false.
        //The boolean only updates when the player lets go of one of the direction keys and holding the other.
        //This will make the charcter move in the last pressed direction by checking what direction the player was moving originally via the boolean.
        //This movement constraint is made because of animation constraints, since there is no diagonal animation for the character.
        /*if (isMovingHorizontal && isMovingVertical)
        {
            if (wasMovingHorizontal)
            {
                verticalMove = Input.GetAxis("Vertical");
                horizontalMove = 0f;
            }
            else
            {
                horizontalMove = Input.GetAxis("Horizontal");
                verticalMove = 0f;
            }
        }
        else if (isMovingHorizontal)
        {
            horizontalMove = Input.GetAxis("Horizontal");
            wasMovingHorizontal = true;
            verticalMove = 0f;
        }
        else if (isMovingVertical)
        {
            verticalMove = Input.GetAxis("Vertical");
            wasMovingHorizontal = false;
            horizontalMove = 0f;
        }*/

        //this will mark what direction the player is facing, 1 for up and right, -1 for left and down
        /*since the fov can point in 8 directions and the player sprite only works for 4, the methods for what
         * direction the player is facing have to be done different*/
        /*cone direction will get input from both directions if player is pressing 2 keys at once else it sets
         * one direction based on the single key pressed and makes the other zero so the cone always faces the
         * direction the player is moving not the direction the sprite is looking*/
        if(Mathf.Abs(horizontalMove) > 0 && Mathf.Abs(verticalMove) > 0)
        {
            coneDirection.x = horizontalMove;
            coneDirection.y = verticalMove;

        }else if (Mathf.Abs(horizontalMove) > 0)
        {
            if (horizontalMove > 0) coneDirection.x = 1;
            else coneDirection.x = -1;
            coneDirection.y = 0;

        }else if(Mathf.Abs(verticalMove) > 0)
        {
            if (verticalMove > 0) coneDirection.y = 1;
            else coneDirection.y = -1;
            coneDirection.x = 0;

        }

        /*look Horizontal & Vertical are for the animator and will only say which of the 4 directions (up,down,left,right)
         * the asset should be facing based on player movement with a preference to horizontal movement*/
        if (horizontalMove > 0)
        {
            lookHorizontal = 1;
            lookVertical = 0;
        }
        else if (horizontalMove < 0)
        {
            lookVertical = 0;
            lookHorizontal = -1;
        }
        else if (verticalMove > 0)
        {
            lookVertical = 1;
            lookHorizontal = 0;
        }
        else if (verticalMove < 0)
        {
            lookVertical = -1;
            lookHorizontal = 0;
        }

        moveDirection = new Vector3(horizontalMove, verticalMove);

        rb.velocity = moveDirection * currentSpeed;
    }

    void Animate()
    {
        //animator.SetFloat("Horizontal", moveDirection.x);
        //animator.SetFloat("Vertical", moveDirection.y);

        //informs animator of player direction
        animator.SetFloat("LookHorizontal", lookHorizontal);
        animator.SetFloat("LookVertical", lookVertical);

        animator.SetFloat("MoveMagnitude", moveDirection.magnitude);//this will tell the animator when the player is moving, so it will switch to moving animations
    }

    public void Damage(int damage)
    {
        playerHealth -= damage;
        damageSound.Play();
        StartCoroutine(turnRed());
    }
    /*Enumerator will change the sprite renderer color from whit to red and back to white to cause the character to flash red*/
    private IEnumerator turnRed()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }
    private void UseItem()
    {
      GameObject buff = GetClosest();
      if (buff == null)
         return;
      if (Vector3.Distance(player.transform.position, buff.transform.position) < 0.3)
      {
         if (useItemText != null)
            useItemText.GetComponent<Text>().enabled = true;

         if (Input.GetKeyDown(KeyCode.E) && !UIManager.isGamePaused)
         {
            // health pack
            if (buff.name.Contains("health") && playerHealth < 6)
            {
               playerHealth = playerHealth + 10;
               Destroy(buff);
            }
            // temporary speed boost
            else if (buff.name.Contains("speed"))
            {
               Destroy(buff);
               isUsingCDTimer = true;
               currentSpeed = 2f;
               StartCoroutine(BuffTime());
            }
            // get weapon
            else if (buff.name.Contains("knife") && !hasWeapon)
            {
               hasWeapon = true;
               weapon = buff;
               weapon.SetActive(false);
            }
         }
      }
      else
      {
         if (useItemText != null)
            useItemText.GetComponent<Text>().enabled = false;
      }
   }

   IEnumerator BuffTime()
   {
      yield return new WaitForSeconds(20);
      currentSpeed = 1.5f;
   }

   private GameObject GetClosest()
   {
      GameObject [] buffs = GameObject.FindGameObjectsWithTag("buff");
      if (buffs.Length == 0)
         return null;
      GameObject nearest = null;
      float min = Mathf.Infinity;
      Vector3 currentPos = player.transform.position;
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

    /* private void OnTriggerEnter2D(Collider2D collision)
     {
         if (collision.gameObject.CompareTag("FOV"))
         {
             fieldOfView = collision.gameObject.GetComponent<FieldOfView>();
         }
     }
    */

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.GetComponent<AssasinationZone>())
        {
            enemy = EnemiesManager.Instance.TryGetAssasinatableEnemy();
            if (killText != null && hasWeapon)
               killText.GetComponent<Text>().enabled = true;
        }
        else
        {
           if (killText != null)
               killText.GetComponent<Text>().enabled = false;
        }
    }

    private void Assassinate(BasicEnemy enemy)
    {
        if (!enemy) return;
        attackSound.Play();
        enemy.Assasinate();
    }

}
