using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick_Example_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    // input variables
    private bool canJump = true;

    // default scale of our sprite
    public Vector3 spriteScale = new Vector3(10, 10, 1);

    // movement and jump variables
    private float maxDistanceToGround = 0;
    private float groundYPos = -2.385f;

    public float speed = 5;
    public float jumpForce = 8;

    public Joystick_Example joystickScript;
    private Vector2 direction;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // continuous check to see how close the player is to the ground
        CheckDistanceToGround();

        movement = joystickScript.GetJoystickOutput();

        Movement();
        Jump();
    }

    private void Movement()
    {
        // only consider this method if we're not jumping
        if (!canJump)
        {
            return;
        }

        CheckSpriteOrientation();

        // since it is a 2D sprite, we only care about horizontal movement (left/right)
        Vector3 translation = new Vector3(movement.x, 0, 0);
        transform.position += translation * (Time.deltaTime * speed);

        // set the float of our animator parameter
        // the method "Mathf.Abs()" is used to return the absolute value of a variable, meaning that it will always return the positive value of a number:
        // - our "Run" parameter expects that values lower than "0.01" mean our character is idle
        // - but in fact our movement values range from -1 to 1, while 0 means we're not moving
        // - so to counter the fact that -1 is considered lower than "0.01" (our animator considers that to be idle), 
        // we need to make sure that it assumes a positive value for movement that returns negative values - SO WE USE "Mathf.Abs()"
        animator.SetFloat("Run", Mathf.Abs(movement.x));
    }

    private void Jump()
    {
        // make sure a new jump is only possible if we're on the ground
        if (transform.position.y <= groundYPos)
        {
            canJump = true;
        } // if not we exit this method
        else
        {
            return;
        }

        CheckSpriteOrientation();

        // only consider swipes that are going significantly upwards
        if (movement.y > 0.5f)
        {
            // make sure we can't jump while on the air and reset variables related to jumping because of the new jump
            canJump = false;
            maxDistanceToGround = 0;

            // Apply the jump impulse (force) on our character
            rb.AddForce(new Vector2(movement.x, 1) * jumpForce, ForceMode2D.Impulse);

            // call the jump animation
            animator.SetTrigger("Jump");
        }
    }

    private void CheckSpriteOrientation()
    {
        if (movement.x > 0)
        {
            // ...we flip our sprite right
            transform.localScale = new Vector3(spriteScale.x, spriteScale.y, spriteScale.z);
        }
        else if (movement.x < 0)
        {
            // ...we flip our sprite left
            transform.localScale = new Vector3(-spriteScale.x, spriteScale.y, spriteScale.z);
        }
    }

    private void CheckDistanceToGround()
    {
        if (!canJump)
        {
            // check if the distance of our character to the ground is the maximum it has ever reached
            if (Mathf.Abs(transform.position.y - groundYPos) >= maxDistanceToGround)
            {
                //... by saving it into a variable and continuously comparing it to the current distance
                maxDistanceToGround = transform.position.y - groundYPos;
            }
            // in the frame that that distance is not bigger, that means we're falling to the ground
            else
            {
                // set the fall animation
                animator.ResetTrigger("Jump");
                animator.SetBool("Fall", true);
            }

            // once we reach the ground again, stop the falling animation
            if (transform.position.y <= groundYPos)
            {
                animator.SetBool("Fall", false);
            }
        }
    }
}
