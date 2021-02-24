using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update :p

    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] float baseSpeed = 3.0f;
    [SerializeField] float currentSpeed; //may or may not have speed potions, delete baseSpeed and assign value to currentSpeed if not.

    [SerializeField] int playerHealth;

    //this bool tells what the orignal direction the player was moving before the new Update() is called.
    private bool wasMovingHorizontal = false;

    private Animator animator;

    private Rigidbody2D rb;

    //making moveDirection a non-local variable allows the code to get its magnitude in Animate()
    private Vector3 moveDirection;

    //shows the direction player is facing
    public float lookVertical = -1, lookHorizontal = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fieldOfView.SetAimDirection(this.GetComponent<PlayerController>());
        playerHealth = 5;

        currentSpeed = baseSpeed;

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
        fieldOfView.SetAimDirection(this.GetComponent<PlayerController>());
        //this will tell fov where to draw the vision cone which will be on the player position
        fieldOfView.SetOrigin(this.transform.position);
        Animate();
    }

    void Move()
    {
        float horizontalMove = 0f;
        float verticalMove = 0f;

        bool isMovingHorizontal = (Input.GetAxisRaw("Horizontal") != 0);
        bool isMovingVertical = (Input.GetAxisRaw("Vertical") != 0);

        //if both keys are being pressed, don't update  wasMovingHorizontal otherwise it will move diagonally in a messed up way.
        //Reason is, once the character moves a tiny bit in one direction then Update() is called changing the variable rapidly switching isMovingHorizontal from true to false.
        //The boolean only updates when the player lets go of one of the direction keys and holding the other.
        //This will make the charcter move in the last pressed direction by checking what direction the player was moving originally via the boolean.
        //This movement constraint is made because of animation constraints, since there is no diagonal animation for the character.
        if (isMovingHorizontal && isMovingVertical)
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
        }

        //this will mark what direction the player is facing, 1 for up and right, -1 for left and down
        if (verticalMove > 0)
        {
            lookVertical = 1;
            lookHorizontal = 0;
        }
        else if (verticalMove < 0)
        {
            lookVertical = -1;
            lookHorizontal = 0;
        }
        else if (horizontalMove > 0)
        {
            lookHorizontal = 1;
            lookVertical = 0;
        }
        else if (horizontalMove < 0)
        {
            lookVertical = 0;
            lookHorizontal = -1;
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

    void takeDamage(int damage)
    {
        playerHealth -= damage;
    }

    void heal()
    {
        playerHealth += 1;
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