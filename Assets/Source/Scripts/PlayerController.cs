using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update :p

    [SerializeField] private Field_of_View fieldOfView;
    [SerializeField] float baseSpeed = 3.0f;
    [SerializeField] float currentSpeed; //may or may not have speed potions, delete baseSpeed and assign value to currentSpeed if not.

    [SerializeField] public static int playerHealth;

    public static bool hasWeapon = false;
    public static GameObject weapon = null;
    private bool speed;


    //this bool tells what the orignal direction the player was moving before the new Update() is called.
    private bool wasMovingHorizontal = false;

    private Animator animator;

    private Rigidbody2D rb;

    //making moveDirection a non-local variable allows the code to get its magnitude in Animate()
    private Vector3 moveDirection;

    //shows the direction player is facing
    private float lookVertical = -1, lookHorizontal = 0;
    private Vector3 coneDirection;
    private Vector3 currentConeDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentConeDirection = new Vector3(0, -1, 0);
        fieldOfView.SetAimDirection(currentConeDirection);
        playerHealth = 5;

        currentSpeed = baseSpeed;
        speed = false;

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
        if (Input.GetMouseButtonDown(0)) Damage(1);
        Move();
        //this will tell the fov what direction the player is facing
        fieldOfView.SetAimDirection(currentConeDirection);
        //this will tell fov where to draw the vision cone which will be on the player position
        fieldOfView.SetOrigin(this.transform.position);
        Animate();
        UseItem();
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
        if(Mathf.Abs(horizontalMove) > 0 && Mathf.Abs(verticalMove) > 0)
        {
            coneDirection.x = horizontalMove;
            coneDirection.y = verticalMove;
            currentConeDirection = coneDirection;

        }else if (Mathf.Abs(horizontalMove) > 0)
        {
            if (horizontalMove > 0) coneDirection.x = 1;
            else coneDirection.x = -1;
            coneDirection.y = 0;
            currentConeDirection = coneDirection;

        }else if(Mathf.Abs(verticalMove) > 0)
        {
            if (verticalMove > 0) coneDirection.y = 1;
            else coneDirection.y = -1;
            coneDirection.x = 0;
            currentConeDirection = coneDirection;

        }

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
        StartCoroutine(turnRed());
    }

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
      if (Input.GetKeyDown(KeyCode.E) && !UIManager.isGamePaused)
      {
         GameObject buff = GetClosest();
         if (buff == null)
         return;
         if (Vector3.Distance(GameObject.Find("JoshuaSpatial").transform.position, buff.transform.position) < 2)
         {
            // health pack
            if (buff.name == "health" && playerHealth < 6)
            {
               playerHealth = playerHealth + 10;
               buff.SetActive(false);
            }
            // temporary speed boost
            else if (buff.name == "speed" && !speed)
            {
               buff.SetActive(false);
               currentSpeed = 4.0f;
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
      currentSpeed = 3.0f;
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

   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FOV"))
        {
            fieldOfView = collision.gameObject.GetComponent<FieldOfView>();
        }
    }
   */
}
